using System.Linq;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.Popups;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Popups;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeReaction;

/// <summary>
/// This handles...
/// </summary>
public sealed class SlimeReactionSystem : EntitySystem
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

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

        bool reactionHappened = false;

        if (reactions == null)
            return;

        var dictContents = contents.ToDictionary(re => re.Reagent.Prototype, re => re.Quantity);

        var dictReactions = reactions.ToDictionary(re => re.Method.ToString(), re => re.Effects);

        var keysInCommon = dictContents.Keys.Intersect<string>(dictReactions.Keys);

        foreach (var key in keysInCommon)
        {
            var effects = dictReactions[key];

            foreach (var effect in effects)
            {
                var effectArgs = new SlimeReagentEffectArgs
                {
                    ExtractEntity = uid,
                    EntityManager = _entManager,
                    RobustRandom = _random
                };

                if (effect.Effect(effectArgs))
                {
                    reactionHappened = true;
                    _audio.PlayPvs(component.ReactionSound, uid);
                }


            }
        }

        if (reactionHappened)
        {
            component.Used = true;
        }

    }
}
