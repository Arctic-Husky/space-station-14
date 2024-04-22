using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.Nutrition.Components;
using Content.Server.Nutrition.EntitySystems;
using Content.Server.Popups;
using Content.Shared._EstacaoPirata.Xenobiology.Meiosis;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeFeeding;
using Content.Shared.Administration.Logs;
using Content.Shared.Body.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Database;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Nutrition;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Verbs;
using Robust.Server.GameObjects;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeFeeding;

/// <summary>
/// This handles...
/// </summary>
public sealed class SlimeFeedingSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly OpenableSystem _openable = default!;
    [Dependency] private readonly SolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly BodySystem _body = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solution = default!;
    [Dependency] private readonly FoodSystem _food = default!;
    [Dependency] private readonly SharedInteractionSystem _interaction = default!;
    [Dependency] private readonly TransformSystem _transform = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly FlavorProfileSystem _flavorProfile = default!;
    //[Dependency] private readonly HungerSystem _hungerSystem = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<SlimeFoodComponent, GetVerbsEvent<InteractionVerb>>(AddFeedVerb);
        SubscribeLocalEvent<SlimeFeedingComponent, ConsumeDoAfterEvent>(OnDoAfter);
    }

    private void AddFeedVerb(Entity<SlimeFoodComponent> entity, ref GetVerbsEvent<InteractionVerb> ev)
    {
        // if (entity.Owner != ev.User)
        //     return;
        if (ev.Target == ev.User)
            return;
        if (!ev.CanInteract)
            return;
        if (!ev.CanAccess)
            return;
        if (!TryComp<BodyComponent>(ev.User, out var body))
            return;
        if (!_body.TryGetBodyOrganComponents<StomachComponent>(ev.User, out var stomachs, body))
            return;
        if (!TryComp<SlimeFeedingComponent>(ev.User, out var slimeFeedingComponent))
            return;

        var user = ev.User;
        var target = ev.Target;
        InteractionVerb verb = new()
        {
            Act = () =>
            {
                Feed(user, target, slimeFeedingComponent);
            },
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/cutlery.svg.192dpi.png")),
            Text = Loc.GetString("food-system-verb-feed"),
            Priority = -1
        };

        ev.Verbs.Add(verb);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<HungerComponent, SlimeFeedingComponent>();
        while (query.MoveNext(out var uid, out var hunger, out var feeding))
        {
            if (_timing.CurTime < feeding.NextUpdateTime)
                continue;

            feeding.NextUpdateTime = _timing.CurTime + feeding.UpdateRate;

            if(hunger.CurrentThreshold < HungerThreshold.Okay)
                continue;

            if (feeding.LastHungerValue <= hunger.CurrentHunger)
            {
                var difference =  hunger.CurrentHunger - feeding.LastHungerValue;
                feeding.FeedingMeter += difference;

                if (feeding.FeedingMeter >= feeding.FeedingLimit)
                {
                    // Fazer meiose ou growth e deletar a entidade original
                    var slimeFedEvent = new SlimeTotallyFedEvent(uid);
                    RaiseLocalEvent(uid, slimeFedEvent);

                    QueueDel(uid);
                    return;
                }
            }
            feeding.LastHungerValue = hunger.CurrentHunger;
        }
    }

    public bool Feed(EntityUid entity, EntityUid target, SlimeFeedingComponent slimeFeedingComponent)
    {
        //Suppresses eating yourself and alive mobs
        if (target == entity || (_mobState.IsAlive(target)))
            return false;

        // Target can't be fed or they're already eating
        if (!TryComp<BodyComponent>(target, out var body))
            return false;

        if (!_body.TryGetBodyOrganComponents<StomachComponent>(entity, out var stomachs))
            return false;

        var nutrimentEntity = Spawn(slimeFeedingComponent.SlimeNutrimentPrototype, _transform.GetMapCoordinates(target));

        if (!TryComp<FoodComponent>(nutrimentEntity, out var foodComponent))
            return false;

        if (!_solutionContainer.TryGetSolution(nutrimentEntity, foodComponent.Solution, out _, out var foodSolution))
            return false;

        var flavors = _flavorProfile.GetLocalizedFlavorsMessage(nutrimentEntity, entity, foodSolution);

        // if (_food.IsMouthBlocked(nutrimentEntity, entity))
        //     return false;

        if (!_interaction.InRangeUnobstructed(entity, target, slimeFeedingComponent.MaxFeedingDistance, popup: true))
            return false;

        if (!_transform.GetMapCoordinates(entity).InRange(_transform.GetMapCoordinates(target), slimeFeedingComponent.MaxFeedingDistance))
        {
            var message = Loc.GetString("interaction-system-user-interaction-cannot-reach");
            _popup.PopupEntity(message, entity, entity);
            return false;
        }

        _adminLogger.Add(LogType.Ingestion, LogImpact.Low, $"{ToPrettyString(entity):entity} is feeding on {ToPrettyString(target):target} {SolutionContainerSystem.ToPrettyString(foodSolution)}");

        var doAfterArgs = new DoAfterArgs(EntityManager, entity, foodComponent.Delay,new ConsumeDoAfterEvent(foodComponent.Solution, flavors), eventTarget: nutrimentEntity, target: target, used: nutrimentEntity)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            MovementThreshold = 0.01f,
            DistanceThreshold = slimeFeedingComponent.MaxFeedingDistance,
            // Mice and the like can eat without hands.
            // TODO maybe set this based on some CanEatWithoutHands event or component?
            NeedHand = false,
        };

        _doAfter.TryStartDoAfter(doAfterArgs);

        return true;
    }

    private void OnDoAfter(Entity<SlimeFeedingComponent> entity, ref ConsumeDoAfterEvent args)
    {
        if (args.Cancelled || args.Handled || entity.Comp.Deleted || args.Target == null)
        {
            QueueDel(args.Used);
            return;
        }

        Log.Debug($"Consumindo {args.Used}");
    }
}
