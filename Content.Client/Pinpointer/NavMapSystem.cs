using Content.Shared.Pinpointer;
using Robust.Shared.GameStates;
<<<<<<< HEAD
=======
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

namespace Content.Client.Pinpointer;

public sealed partial class NavMapSystem : SharedNavMapSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<NavMapComponent, ComponentHandleState>(OnHandleState);
    }

    private void OnHandleState(EntityUid uid, NavMapComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not NavMapComponentState state)
            return;

        if (!state.FullState)
        {
            foreach (var index in component.Chunks.Keys)
            {
                if (!state.AllChunks!.Contains(index))
                    component.Chunks.Remove(index);
            }
        }
<<<<<<< HEAD
        else
        {
            foreach (var index in component.Chunks.Keys)
            {
                if (!state.Chunks.ContainsKey(index))
                    component.Chunks.Remove(index);
=======

        component.Beacons.Clear();
        component.Beacons.AddRange(state.Beacons);

        component.Airlocks.Clear();
        component.Airlocks.AddRange(state.Airlocks);
    }
}

public sealed class NavMapOverlay : Overlay
{
    private readonly IEntityManager _entManager;
    private readonly IMapManager _mapManager;

    public override OverlaySpace Space => OverlaySpace.WorldSpace;

    private List<Entity<MapGridComponent>> _grids = new();

    public NavMapOverlay(IEntityManager entManager, IMapManager mapManager)
    {
        _entManager = entManager;
        _mapManager = mapManager;
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        var query = _entManager.GetEntityQuery<NavMapComponent>();
        var xformQuery = _entManager.GetEntityQuery<TransformComponent>();
        var scale = Matrix3.CreateScale(new Vector2(1f, 1f));

        _grids.Clear();
        _mapManager.FindGridsIntersecting(args.MapId, args.WorldBounds, ref _grids);

        foreach (var grid in _grids)
        {
            if (!query.TryGetComponent(grid, out var navMap) || !xformQuery.TryGetComponent(grid.Owner, out var xform))
                continue;

            // TODO: Faster helper method
            var (_, _, matrix, invMatrix) = xform.GetWorldPositionRotationMatrixWithInv();

            var localAABB = invMatrix.TransformBox(args.WorldBounds);
            Matrix3.Multiply(in scale, in matrix, out var matty);

            args.WorldHandle.SetTransform(matty);

            for (var x = Math.Floor(localAABB.Left); x <= Math.Ceiling(localAABB.Right); x += SharedNavMapSystem.ChunkSize * grid.Comp.TileSize)
            {
                for (var y = Math.Floor(localAABB.Bottom); y <= Math.Ceiling(localAABB.Top); y += SharedNavMapSystem.ChunkSize * grid.Comp.TileSize)
                {
                    var floored = new Vector2i((int) x, (int) y);

                    var chunkOrigin = SharedMapSystem.GetChunkIndices(floored, SharedNavMapSystem.ChunkSize);

                    if (!navMap.Chunks.TryGetValue(chunkOrigin, out var chunk))
                        continue;

                    // TODO: Okay maybe I should just use ushorts lmao...
                    for (var i = 0; i < SharedNavMapSystem.ChunkSize * SharedNavMapSystem.ChunkSize; i++)
                    {
                        var value = (int) Math.Pow(2, i);

                        var mask = chunk.TileData & value;

                        if (mask == 0x0)
                            continue;

                        var tile = chunk.Origin * SharedNavMapSystem.ChunkSize + SharedNavMapSystem.GetTile(mask);
                        args.WorldHandle.DrawRect(new Box2(tile * grid.Comp.TileSize, (tile + 1) * grid.Comp.TileSize), Color.Aqua, false);
                    }
                }
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            }
        }

        foreach (var (origin, chunk) in state.Chunks)
        {
            var newChunk = new NavMapChunk(origin);
            Array.Copy(chunk, newChunk.TileData, chunk.Length);
            component.Chunks[origin] = newChunk;
        }

        component.Beacons.Clear();
        foreach (var (nuid, beacon) in state.Beacons)
        {
            component.Beacons[nuid] = beacon;
        }
    }
}
