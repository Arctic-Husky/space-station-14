using Content.Shared.Explosion.Components;
using Content.Shared.Explosion.EntitySystems;
using Content.Server.Fluids.EntitySystems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Coordinates.Helpers;
using Content.Shared.Maps;
<<<<<<< HEAD
using Robust.Server.GameObjects;
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
using Robust.Shared.Map;

namespace Content.Server.Explosion.EntitySystems;

/// <summary>
/// Handles creating smoke when <see cref="SmokeOnTriggerComponent"/> is triggered.
/// </summary>
public sealed class SmokeOnTriggerSystem : SharedSmokeOnTriggerSystem
{
    [Dependency] private readonly IMapManager _mapMan = default!;
    [Dependency] private readonly SmokeSystem _smoke = default!;
<<<<<<< HEAD
    [Dependency] private readonly TransformSystem _transform = default!;
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SmokeOnTriggerComponent, TriggerEvent>(OnTrigger);
    }

    private void OnTrigger(EntityUid uid, SmokeOnTriggerComponent comp, TriggerEvent args)
    {
        var xform = Transform(uid);
<<<<<<< HEAD
        var mapCoords = _transform.GetMapCoordinates(uid, xform);
        if (!_mapMan.TryFindGridAt(mapCoords, out _, out var grid) ||
=======
        if (!_mapMan.TryFindGridAt(xform.MapPosition, out _, out var grid) ||
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            !grid.TryGetTileRef(xform.Coordinates, out var tileRef) ||
            tileRef.Tile.IsSpace())
        {
            return;
        }

<<<<<<< HEAD
        var coords = grid.MapToGrid(mapCoords);
=======
        var coords = grid.MapToGrid(xform.MapPosition);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        var ent = Spawn(comp.SmokePrototype, coords.SnapToGrid());
        if (!TryComp<SmokeComponent>(ent, out var smoke))
        {
            Log.Error($"Smoke prototype {comp.SmokePrototype} was missing SmokeComponent");
            Del(ent);
            return;
        }

        _smoke.StartSmoke(ent, comp.Solution, comp.Duration, comp.SpreadAmount, smoke);
    }
}
