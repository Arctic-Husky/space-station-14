using System.Linq;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Robust.Shared.Random;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeReaction;

/// <summary>
/// This handles...
/// </summary>
public sealed class SlimeReactionSystem : EntitySystem
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SolutionContainerSystem _solutionContainer = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SlimeReactionComponent, SolutionContainerChangedEvent>(OnSolutionChanged);
    }

    // TODO: fazer algo pra impedir injecting caso ja esteja usado, ou mudar de lugar a mensagem
    private void OnSolutionChanged(EntityUid uid, SlimeReactionComponent component, ref SolutionContainerChangedEvent args)
    {
        Log.Debug($"SolutionContainerChangedEvent");

        // Really annoying that this gets run when the entity spawns
        if (component.ExtractJustSpawned)
        {
            component.ExtractJustSpawned = false;
            return;
        }

        // The extract has already created a reaction before and will not create any more reactions
        if (component.Used)
        {
            // TODO: De algum jeito vou precisar pegar a entidade que usou o injetor no extract pra exibir so pra ela
            // var message = Loc.GetString("slime-extract-already-used");
            // _popup.PopupEntity(message, uid, PopupType.Small);

            return;
        }

        var contents = args.Solution.Contents;

        var reactions = component.Reactions;

        if (reactions == null)
            return;

        var dictContents = contents.ToDictionary(re => re.Reagent.Prototype, re => re.Quantity);

        var dictReactions = reactions.ToDictionary(re => re.Method.ToString(), re => re.Effects);

        var keysInCommon = dictContents.Keys.Intersect<string>(dictReactions.Keys);

        var inCommon = keysInCommon.ToList();
        if (inCommon.Count > 0)
        {
            var activeSlimeReactionComponent = EnsureComp<ActiveSlimeReactionComponent>(uid);

            foreach (var key in inCommon)
            {
                var effects = dictReactions[key];

                foreach (var effect in effects)
                {
                    var effectArgs = new SlimeReagentEffectArgs
                    {
                        ExtractEntity = uid,
                        EntityManager = _entManager,
                        RobustRandom = _random,
                        Quantity = args.Solution.Contents.Find(reagent => reagent.Reagent.Prototype == key).Quantity,
                        ReactionComponent = component
                    };

                    activeSlimeReactionComponent.Effects.Add(effect, effectArgs);

                    activeSlimeReactionComponent.WaitTime = effect.NeedsTime();

                    ClearSolution(uid, component);
                }
            }
        }
    }

    // TODO: mudar isto para usar as coisas que vem do args
    private void ClearSolution(EntityUid uid, SlimeReactionComponent component)
    {
        if (TryComp<SolutionContainerManagerComponent>(uid, out var solutionContainerManagerComponent))
        {
            var entity = new Entity<SolutionContainerManagerComponent?>(uid, solutionContainerManagerComponent);

            if (_solutionContainer.TryGetSolution(entity, component.SolutionName, out var soln))
            {
                soln.Value.Comp.Solution.RemoveAllSolution();
            }
        }
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<ActiveSlimeReactionComponent, SlimeReactionComponent>();
        while (query.MoveNext(out var uid, out var activeComp, out var reactionComp))
        {
            if (activeComp.WaitTime > 0)
            {
                activeComp.WaitTime -= frameTime;
                continue;
            }

            if (reactionComp.Used)
            {
                continue;
            }

            if (activeComp.Effects.Any())
            {
                var effects = activeComp.Effects;
                foreach (var effect in effects)
                {
                    // Se ocorreu o efeito
                    if (effect.Key.Effect(effect.Value))
                    {
                        reactionComp.Used = true;
                        RemCompDeferred<ActiveSlimeReactionComponent>(uid);
                    }
                }
            }
        }
    }
}
