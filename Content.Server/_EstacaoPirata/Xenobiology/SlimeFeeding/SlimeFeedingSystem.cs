using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.Popups;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeFeeding;
using Content.Shared.Actions.Events;
using Content.Shared.Administration.Logs;
using Content.Shared.Body.Components;
using Content.Shared.Chemistry;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.CombatMode;
using Content.Shared.Damage;
using Content.Shared.Database;
using Content.Shared.DoAfter;
using Content.Shared.FixedPoint;
using Content.Shared.Interaction;
using Content.Shared.Mobs.Systems;
using Content.Shared.Nutrition.Components;
using Content.Shared.Verbs;
using Content.Shared.Weapons.Melee.Events;
using Robust.Server.GameObjects;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeFeeding;

/// <summary>
/// This handles...
/// </summary>
public sealed class SlimeFeedingSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming                        _timing = default!;
    [Dependency] private readonly BodySystem                           _body = default!;
    [Dependency] private readonly SharedInteractionSystem       _interaction = default!;
    [Dependency] private readonly TransformSystem                 _transform = default!;
    [Dependency] private readonly PopupSystem                         _popup = default!;
    [Dependency] private readonly ISharedAdminLogManager        _adminLogger = default!;
    [Dependency] private readonly SharedDoAfterSystem               _doAfter = default!;
    [Dependency] private readonly SharedSlimeLeapingSystem    _leapingSystem = default!;
    [Dependency] private readonly StomachSystem                     _stomach = default!;
    [Dependency] private readonly ReactiveSystem                   _reaction = default!;
    [Dependency] private readonly SolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly DamageableSystem               _damageable = default!;
    [Dependency] private readonly MobStateSystem                   _mobState = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<SlimeFoodComponent, GetVerbsEvent<AlternativeVerb>>(AddFeedVerb);
        SubscribeLocalEvent<SlimeFeedingComponent, FeedDoAfterEvent>(OnDoAfter);
        SubscribeLocalEvent<SlimeFeedingComponent, LatchOnEvent>(OnFeed);
        SubscribeLocalEvent<SlimeFeedingComponent, DisarmedEvent>(OnDisarmed);
        SubscribeLocalEvent<SlimeFeedingComponent, ComponentShutdown>(OnShutdown);
    }
    private void AddFeedVerb(Entity<SlimeFoodComponent> entity, ref GetVerbsEvent<AlternativeVerb> ev)
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
        AlternativeVerb verb = new()
        {
            Act = () =>
            {
                _leapingSystem.LeapToTarget(user, target);
            },
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/cutlery.svg.192dpi.png")),
            Text = Loc.GetString("slime-system-verb-feed"),
            Priority = 10
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
                    Log.Debug($"Slime {uid} totalmente alimentado");
                    var slimeFedEvent = new SlimeTotallyFedEvent(uid);
                    RaiseLocalEvent(uid, slimeFedEvent);

                    // Dar unlatch caso tenha vitima antes de deletar a entidade
                    if (feeding.Victim != null)
                    {
                        RaiseUnlatchEvent(uid, feeding.Victim.Value);
                    }

                    Log.Debug($"Deletando {uid}");
                    QueueDel(uid);
                    return;
                }
            }
            feeding.LastHungerValue = hunger.CurrentHunger;
        }
    }

    private void OnFeed(EntityUid uid, SlimeFeedingComponent component, LatchOnEvent args)
    {
        Log.Debug($"OnFeed");
        TryFeed(args.User, args.Target, component);
    }

    public bool TryFeed(EntityUid entity, EntityUid target)
    {
        //Resolve()
        if(!TryComp<SlimeFeedingComponent>(entity, out var slimeFeedingComponent))
            return false;

        return TryFeed(entity, target, slimeFeedingComponent);
    }

    public bool TryFeed(EntityUid entity, EntityUid target, SlimeFeedingComponent slimeFeedingComponent)
    {
        //Suppresses eating yourself
        if (target == entity)
        {
            Log.Debug($"target == entity");
            return false;
        }


        if (!TryComp<SlimeFoodComponent>(target, out var slimeFoodComponent))
        {
            Log.Debug($"Sem slimeFoodComponent");
            RaiseUnlatchEvent(entity, target);
            return false;
        }

        if (HasComp<SlimeBadFoodComponent>(target))
        {
            Log.Debug($"Bad Food");
            RaiseUnlatchEvent(entity, target);
            return false;
        }


        if (slimeFoodComponent.Remaining <= 0)
        {
            Log.Debug($"Sem recursos o suficiente no alvo, saindo");
            var message = Loc.GetString("slime-food-not-enough-resources-on-target");
            _popup.PopupEntity(message, entity, entity);
            RaiseUnlatchEvent(entity, target);
            return false;
        }


        // Target can't be fed or they're already eating
        if (!TryComp<BodyComponent>(target, out var body))
        {
            Log.Debug($"Sem corpo");
            RaiseUnlatchEvent(entity, target);
            return false;
        }

        if (!_body.TryGetBodyOrganComponents<StomachComponent>(entity, out var stomachs))
        {
            Log.Debug($"Sem estomagos");
            RaiseUnlatchEvent(entity, target);
            return false;
        }

        if (!_interaction.InRangeUnobstructed(entity, target, slimeFeedingComponent.MaxFeedingDistance, popup: true))
        {
            Log.Debug($"InRangeUnobstructed deu ruim");
            RaiseUnlatchEvent(entity, target);
            return false;
        }

        if (!_transform.GetMapCoordinates(entity).InRange(_transform.GetMapCoordinates(target), slimeFeedingComponent.MaxFeedingDistance))
        {
            Log.Debug($"Sem alcance, saindo");
            var message = Loc.GetString("interaction-system-user-interaction-cannot-reach");
            _popup.PopupEntity(message, entity, entity);
            RaiseUnlatchEvent(entity, target);
            return false;
        }

        _adminLogger.Add(LogType.Ingestion, LogImpact.Low, $"{ToPrettyString(entity):entity} is feeding on {ToPrettyString(target):target}");

        var feedDoAfterEvent = new FeedDoAfterEvent();
        feedDoAfterEvent.Repeat = true;

        var doAfterArgs = new DoAfterArgs(
            EntityManager,
            entity,
            slimeFeedingComponent.FeedingTime,
            feedDoAfterEvent,
            eventTarget: entity,
            target: target,
            used: target)
        {
            BreakOnMove = true,
            BreakOnDamage = false,
            MovementThreshold = 0.8f,
            DistanceThreshold = slimeFeedingComponent.MaxFeedingDistance,
            NeedHand = false
        };

        Log.Debug($"Iniciando doafter {entity} -> {target}");

        return _doAfter.TryStartDoAfter(doAfterArgs);
    }

    private bool Feed(EntityUid entity, EntityUid target, SlimeFeedingComponent slimeFeedingComponent)
    {
        Log.Debug($"Feed");

        if (!_mobState.IsAlive(entity))
            return false;

        if (slimeFeedingComponent.VictimResisted)
        {
            Log.Debug($"Vitima resistiu, saindo");
            var message = Loc.GetString("slime-victim-resisted");
            _popup.PopupEntity(message, entity, entity);
            return false;
        }

        if (!TryComp<SlimeFoodComponent>(target, out var slimeFoodComponent))
            return false;

        // See if the victim still has food value
        if (slimeFoodComponent.Remaining <= 0)
        {
            Log.Debug($"Sem recursos no alvo, saindo (feed)");
            var message = Loc.GetString("slime-food-target-drained");
            _popup.PopupEntity(message, entity, entity);
            RaiseUnlatchEvent(entity,target);
            return false;
        }

        if (!TryComp<BodyComponent>(entity, out var body))
            return false;

        if (!_body.TryGetBodyOrganComponents<StomachComponent>(entity, out var stomachs, body))
            return false;

        var drained = new Solution();
        drained.AddReagent(slimeFeedingComponent.FeedingSolutionReagent, slimeFeedingComponent.FeedingSolutionQuantity);

        // Get the stomach with the highest available solution volume
        var highestAvailable = FixedPoint2.Zero;
        StomachComponent? stomachToUse = null;
        foreach (var (stomach, _) in stomachs)
        {
            var owner = stomach.Owner;
            if (!_stomach.CanTransferSolution(owner, drained, stomach))
                continue;

            if (!_solutionContainer.ResolveSolution(owner, StomachSystem.DefaultSolutionName, ref stomach.Solution, out var stomachSol))
                continue;

            if (stomachSol.AvailableVolume <= highestAvailable)
                continue;

            stomachToUse = stomach;
            highestAvailable = stomachSol.AvailableVolume;
        }

        _reaction.DoEntityReaction(entity, drained, ReactionMethod.Ingestion);

        // No stomach so just popup a message that they can't eat and unlatch.
        if (stomachToUse == null)
        {
            slimeFeedingComponent.StomachAvailable = false;
            //_solutionContainer.TryAddSolution(soln.Value, drained);
            Log.Debug($"Estomago cheio, saindo");
            _popup.PopupEntity(Loc.GetString("food-system-you-cannot-eat-any-more"), entity, entity);
            RaiseUnlatchEvent(entity, target);
            return false;
        }

        slimeFeedingComponent.StomachAvailable = true;

        // Consume the solution
        if (_stomach.TryTransferSolution(stomachToUse.Owner, drained, stomachToUse))
        {
            var damageResult = _damageable.TryChangeDamage(target, slimeFeedingComponent.FeedingDamage, origin:entity);
            Log.Debug($"Comi com sucesso, saindo");
            var message = Loc.GetString("slime-slurp");
            _popup.PopupEntity(message, entity, entity);

            var victimMessage = Loc.GetString("slime-attacking-you");
            _popup.PopupEntity(victimMessage, target, target);
            slimeFoodComponent.Remaining -= slimeFeedingComponent.FeedingSolutionQuantity.Value/100f;

            // Tocar um som de slurp?

            return true;
        }

        return false;
    }

    private void OnDoAfter(EntityUid uid, SlimeFeedingComponent component, ref FeedDoAfterEvent args)
    {
        Log.Debug($"Pos DoAfter Antes do Feed");
        args.Repeat = true;

        if (args.Cancelled || args.Handled || component.Deleted || args.FeedCancelled)
        {
            //QueueDel(args.Used);
            if (args.Target != null)
            {
                Log.Debug($"Cancelou e args.Target nao e nulo");
                args.FeedCancelled = true;
                args.Repeat = false;
                RaiseUnlatchEvent(args.User, args.Target.Value);
            }
            return;
        }
        if (args.Target == null) // Tenho que ver isto
        {
            Log.Debug($"args.Target e nulo");
            return;
        }

        if(TryComp<SlimeFoodComponent>(args.Target.Value, out var slimeFoodComponent))
        {
            if (slimeFoodComponent.Remaining <= 0)
            {
                Log.Debug($"Vitima drenada completamente");
                args.FeedCancelled = true;
                args.Repeat = false;
                RaiseUnlatchEvent(args.User, args.Target.Value);
                return;
            }
        }

        // Finalmente, feed. See retornar falso, parar o doafter
        // TODO: ver se tem como limpar o codigo de cancel logo acima pra usar so isto daqui
        if (!Feed(args.User, args.Target.Value, component))
        {
            Log.Debug($"Deu ruim no feed");
            args.FeedCancelled = true;
            args.Repeat = false;
            RaiseUnlatchEvent(args.User, args.Target.Value);
            return;
        }
    }

    private void OnDisarmed(EntityUid uid, SlimeFeedingComponent component, ref DisarmedEvent args)
    {
        Log.Debug($"OnDisarmAttempt");

        if (!TryComp<SlimeFeedingIncapacitatedComponent>(args.Source, out var slimeFeedingIncapacitatedComponent))
            return;

        if (args.Target != slimeFeedingIncapacitatedComponent.Attacker)
            return;

        component.VictimResisted = true;
        RaiseUnlatchEvent(args.Target, args.Source);

    }

    private void RaiseUnlatchEvent(EntityUid entity, EntityUid target)
    {
        Log.Debug($"Raise Unlatch Event");
        var unlatchOnEvent = new UnlatchOnEvent(entity, target);
        RaiseLocalEvent(uid: entity, args:unlatchOnEvent);
    }

    private void OnShutdown(EntityUid uid, SlimeFeedingComponent component, ComponentShutdown args)
    {
        Log.Debug($"Shutdown");

        // Isso aqui trata o case de se o slime for deletado de repente
        if (component.Victim is not null)
        {
            RaiseUnlatchEvent(uid, component.Victim.Value);
        }
    }
}
