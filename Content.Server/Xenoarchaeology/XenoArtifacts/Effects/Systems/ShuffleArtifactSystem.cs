using Content.Server.Xenoarchaeology.XenoArtifacts.Effects.Components;
using Content.Server.Xenoarchaeology.XenoArtifacts.Events;
using Content.Shared.Mobs.Components;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server.Xenoarchaeology.XenoArtifacts.Effects.Systems;

public sealed class ShuffleArtifactSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedTransformSystem _xform = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<ShuffleArtifactComponent, ArtifactActivatedEvent>(OnActivated);
    }

    private void OnActivated(EntityUid uid, ShuffleArtifactComponent component, ArtifactActivatedEvent args)
    {
        var mobState = GetEntityQuery<MobStateComponent>();

<<<<<<< HEAD
=======
        List<EntityCoordinates> allCoords = new();
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        List<Entity<TransformComponent>> toShuffle = new();

        foreach (var ent in _lookup.GetEntitiesInRange(uid, component.Radius, LookupFlags.Dynamic | LookupFlags.Sundries))
        {
            if (!mobState.HasComponent(ent))
                continue;

            var xform = Transform(ent);

            toShuffle.Add((ent, xform));
<<<<<<< HEAD
=======
            allCoords.Add(xform.Coordinates);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        }

        _random.Shuffle(toShuffle);

        while (toShuffle.Count > 1)
        {
            var ent1 = _random.PickAndTake(toShuffle);
            var ent2 = _random.PickAndTake(toShuffle);
            _xform.SwapPositions((ent1, ent1), (ent2, ent2));
        }
    }
}
