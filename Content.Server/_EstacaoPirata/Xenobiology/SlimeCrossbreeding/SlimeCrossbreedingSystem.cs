using System.Linq;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeCrossbreeding;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Shared.Audio;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeCrossbreeding;

/// <summary>
/// This handles...
/// </summary>
public sealed class SlimeCrossbreedingSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<SlimeCrossbreedingComponent, InteractUsingEvent>(OnInteraction);
    }

    // Interaction for crossbreeding
    private void OnInteraction(Entity<SlimeCrossbreedingComponent> ent, ref InteractUsingEvent args)
    {
        // verificar se o slime esta morto?

        if (!TryComp<SlimeReactionComponent>(args.Used, out var extract))
            return;

        if (!TryComp<SlimeCrossbreedingComponent>(args.Target, out var target))
            return;

        if (target.Crossbreeds is null)
            return;

        // Extract.Color pode ser vazio, lidar com isso

        if (extract.Color.Length == 0)
        {
            return;
        }

        if (!target.Crossbreeds.TryGetValue(extract.Color, out var value)) // Ja obtem o entry da cor certa
            return;

        if (!ent.Comp.ExtractsUsed.TryAdd(extract.Color, 1))
        {
            if (ent.Comp.ExtractsUsed.TryGetValue(extract.Color, out var timesUsed))
            {
                var newTimesUsed = timesUsed + 1;
                ent.Comp.ExtractsUsed[extract.Color] = newTimesUsed;
                if (newTimesUsed >= ent.Comp.Max)
                {
                    ent.Comp.MaxAchieved = true;
                }
            }
        }

        _popup.PopupEntity(Loc.GetString("core-inserted-into-slime"), args.Target);

        _audio.PlayPvs(ent.Comp.Sound, _transform.GetMoverCoordinates(args.Target), AudioHelpers.WithVariation(0.05f, _robustRandom));

        if (ent.Comp.MaxAchieved)
        {
            var coords = _transform.GetMapCoordinates(args.Target);

            var entity = Spawn(value.Prototype, coords);

            var netEntity = GetNetEntity(entity);

            RaiseNetworkEvent(new ExtractColorChangeEvent(netEntity, value.Color));

            _audio.PlayPvs(ent.Comp.SoundComplete, _transform.GetMoverCoordinates(args.Target), AudioHelpers.WithVariation(0.05f, _robustRandom));

            var component = new SlimeReactionComponent
            {
                Reactions = value.Reactions,
                ExtractJustSpawned = false
            };

            AddComp(entity, component);

            QueueDel(args.Target);
        }

        QueueDel(args.Used);
    }
}
