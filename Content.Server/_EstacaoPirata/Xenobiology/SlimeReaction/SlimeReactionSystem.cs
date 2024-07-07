using System.Linq;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Shared.Audio;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.FixedPoint;
using Content.Shared.Popups;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeReaction;

/// <summary>
/// This handles...
/// </summary>
public sealed class SlimeReactionSystem : EntitySystem
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedAmbientSoundSystem _ambientSoundSystem = default!;

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

        // The extract has already created a reaction and will not create any more reactions
        if (component.Spent) // && component.TimeToWait <= 0
        {
            return;
        }

        var reactions = component.Reactions;

        if (reactions == null)
            return;

        var contents = args.Solution.Contents;

        var dictContents = contents.ToDictionary(re => re.Reagent.Prototype, re => re.Quantity);

        var dictReactions = reactions.ToDictionary(re => re.Method.ToString(), re => (re.Effects, re.Sound));

        var keysInCommon = dictContents.Keys.Intersect<string>(dictReactions.Keys);

        var inCommon = keysInCommon.ToList();
        if (inCommon.Count > 0)
        {
            var activeSlimeReactionComponent = EnsureComp<ActiveSlimeReactionComponent>(uid);

            foreach (var key in inCommon)
            {
                var effects = dictReactions[key];

                foreach (var effect in effects.Effects)
                {
                    // Problematico?
                    if (activeSlimeReactionComponent.Effects.ContainsKey(effect))
                    {
                        continue;
                    }

                    var quantity = args.Solution.Contents.Find(reagent => reagent.Reagent.Prototype == key).Quantity;

                    var effectArgs = new SlimeReagentEffectArgs
                    {
                        ExtractEntity = uid,
                        EntityManager = _entManager,
                        RobustRandom = _random,
                        Quantity = quantity,
                        ReactionComponent = component,
                        Sound = effects.Sound
                    };

                    activeSlimeReactionComponent.Effects.Add(effect, effectArgs);

                    activeSlimeReactionComponent.WaitTime = effect.TimeNeeded();

                    activeSlimeReactionComponent.SpendOnUse = effect.SpendOnUse();

                    RemoveReagent(uid, component, key, quantity);
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

    private void RemoveReagent(EntityUid uid, SlimeReactionComponent component, string prototype, FixedPoint2 quantity)
    {
        if (TryComp<SolutionContainerManagerComponent>(uid, out var solutionContainerManagerComponent))
        {
            var entity = new Entity<SolutionContainerManagerComponent?>(uid, solutionContainerManagerComponent);

            if (_solutionContainer.TryGetSolution(entity, component.SolutionName, out var soln))
            {
                soln.Value.Comp.Solution.RemoveReagent(prototype, quantity);
            }
        }
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<ActiveSlimeReactionComponent, SlimeReactionComponent>();
        while (query.MoveNext(out var uid, out var activeComp, out var reactionComp))
        {
            // bloco inutil
            if (reactionComp.Spent)
            {
                continue;
            }

            if (activeComp.WaitTime > 0)
            {
                if (!activeComp.SoundPlayed)
                {
                    _audio.PlayPvs(reactionComp.ReactionSound, uid);
                    _popupSystem.PopupEntity(Loc.GetString("extract-reaction-activated", ("extract", uid)), uid, PopupType.Small);
                    activeComp.SoundPlayed = true;
                    _ambientSoundSystem.SetAmbience(uid, true);
                }

                activeComp.WaitTime -= frameTime;
                continue;
            }

            if (activeComp.Effects.Count > 0)
            {
                var effects = activeComp.Effects;
                foreach (var effect in effects)
                {
                    // Se ocorreu o efeito
                    if (effect.Key.Effect(effect.Value))
                    {
                        _ambientSoundSystem.SetAmbience(uid, false); // tirar daqui e deixar so la em reactionsuccess
                        _popupSystem.PopupEntity(Loc.GetString("extract-reaction-successful", ("extract", uid), ("occured", Loc.GetString(effect.Key.GetReactionMessage()))), uid, PopupType.Small);
                        _audio.PlayPvs(effect.Value.Sound, uid);
                        activeComp.ReactionSuccess = true;
                        effects.Remove(effect.Key);
                    }
                }
            }

            // TODO: mudar o sprite do extract pra um que indique que ele ja esta esgotado

            // bloco para remover pq nao to lembrando a utilidade
            if (activeComp.SpendOnUse)
            {
                reactionComp.Spent = true;
                RemCompDeferred<ActiveSlimeReactionComponent>(uid);
            }

            if (activeComp.WaitTimeToDeactivate > 0)
            {
                activeComp.WaitTimeToDeactivate -= frameTime;
                continue;
            }

            if (activeComp.ReactionSuccess)
            {
                activeComp.WaitTimeToDeactivate = activeComp.MaxWaitTimeToDeactivate; // Linha inutil

                reactionComp.Spent = true;

                _ambientSoundSystem.SetAmbience(uid, false);
                RemCompDeferred<ActiveSlimeReactionComponent>(uid);
                //return;
            }
        }
    }

    public FormattedMessage GetReactionsText(EntityUid uid)
    {
        var msg = new FormattedMessage();

        if (!TryComp<SlimeReactionComponent>(uid, out var component))
        {
            return msg;
        }

        var reactions = component.Reactions;

        if (reactions is not null)
        {
            foreach (var reaction in reactions)
            {
                var text = reaction.Method.ToString().ToLower();
                var locString = $"reagent-name-{text}";
                msg.AddMarkup(Loc.GetString(locString));
                msg.PushNewline();

            }
        }

        return msg;
    }

    public List<string> GetReactionsList(EntityUid uid)
    {
        var msg = new List<string>();

        if (!TryComp<SlimeReactionComponent>(uid, out var component))
        {
            return msg;
        }

        var reactions = component.Reactions;

        if (reactions is not null)
        {
            foreach (var reaction in reactions)
            {
                var text = reaction.Method.ToString().ToLower();
                var locString = $"reagent-name-{text}";
                msg.Add(locString);
            }
        }

        return msg;
    }
}
