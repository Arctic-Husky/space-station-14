<<<<<<< HEAD
using Content.Server.Administration.Logs;
using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
=======
using System.Diagnostics.CodeAnalysis;
using Content.Server.Administration.Logs;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
using Content.Server.Station.Systems;
using Content.Server.Warps;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.Localizations;
<<<<<<< HEAD
using Content.Shared.Maps;
using Content.Shared.Pinpointer;
using JetBrains.Annotations;
=======
using Content.Shared.Pinpointer;
using Content.Shared.Tag;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Shared.GameStates;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Timing;
using System.Diagnostics.CodeAnalysis;

namespace Content.Server.Pinpointer;

/// <summary>
/// Handles data to be used for in-grid map displays.
/// </summary>
public sealed partial class NavMapSystem : SharedNavMapSystem
{
    [Dependency] private readonly IAdminLogManager _adminLog = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
<<<<<<< HEAD
    [Dependency] private readonly SharedMapSystem _mapSystem = default!;
    [Dependency] private readonly SharedTransformSystem _transformSystem = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly ITileDefinitionManager _tileDefManager = default!;

    public const float CloseDistance = 15f;
    public const float FarDistance = 30f;

    private EntityQuery<AirtightComponent> _airtightQuery;
    private EntityQuery<MapGridComponent> _gridQuery;
    private EntityQuery<NavMapComponent> _navQuery;
=======
    [Dependency] private readonly TagSystem _tags = default!;
    [Dependency] private readonly MapSystem _map = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly TransformSystem _transform = default!;

    private EntityQuery<PhysicsComponent> _physicsQuery;
    private EntityQuery<TagComponent> _tagQuery;

    public const float CloseDistance = 15f;
    public const float FarDistance = 30f;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    public override void Initialize()
    {
        base.Initialize();

<<<<<<< HEAD
        var categories = Enum.GetNames(typeof(NavMapChunkType)).Length - 1; // -1 due to "Invalid" entry.
        if (Categories != categories)
            throw new Exception($"{nameof(Categories)} must be equal to the number of chunk types");

        _airtightQuery = GetEntityQuery<AirtightComponent>();
        _gridQuery = GetEntityQuery<MapGridComponent>();
        _navQuery = GetEntityQuery<NavMapComponent>();

        // Initialization events
        SubscribeLocalEvent<StationGridAddedEvent>(OnStationInit);

        // Grid change events
        SubscribeLocalEvent<GridSplitEvent>(OnNavMapSplit);
        SubscribeLocalEvent<TileChangedEvent>(OnTileChanged);

        SubscribeLocalEvent<AirtightChanged>(OnAirtightChange);

        // Beacon events
        SubscribeLocalEvent<NavMapBeaconComponent, MapInitEvent>(OnNavMapBeaconMapInit);
        SubscribeLocalEvent<NavMapBeaconComponent, AnchorStateChangedEvent>(OnNavMapBeaconAnchor);
=======
        _physicsQuery = GetEntityQuery<PhysicsComponent>();
        _tagQuery = GetEntityQuery<TagComponent>();

        SubscribeLocalEvent<AnchorStateChangedEvent>(OnAnchorChange);
        SubscribeLocalEvent<ReAnchorEvent>(OnReAnchor);
        SubscribeLocalEvent<StationGridAddedEvent>(OnStationInit);
        SubscribeLocalEvent<NavMapComponent, ComponentStartup>(OnNavMapStartup);
        SubscribeLocalEvent<NavMapComponent, ComponentGetState>(OnGetState);
        SubscribeLocalEvent<GridSplitEvent>(OnNavMapSplit);

        SubscribeLocalEvent<NavMapBeaconComponent, MapInitEvent>(OnNavMapBeaconMapInit);
        SubscribeLocalEvent<NavMapBeaconComponent, ComponentStartup>(OnNavMapBeaconStartup);
        SubscribeLocalEvent<NavMapBeaconComponent, AnchorStateChangedEvent>(OnNavMapBeaconAnchor);

        SubscribeLocalEvent<NavMapDoorComponent, ComponentStartup>(OnNavMapDoorStartup);
        SubscribeLocalEvent<NavMapDoorComponent, AnchorStateChangedEvent>(OnNavMapDoorAnchor);

>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        SubscribeLocalEvent<ConfigurableNavMapBeaconComponent, NavMapBeaconConfigureBuiMessage>(OnConfigureMessage);
        SubscribeLocalEvent<ConfigurableNavMapBeaconComponent, MapInitEvent>(OnConfigurableMapInit);
        SubscribeLocalEvent<ConfigurableNavMapBeaconComponent, ExaminedEvent>(OnConfigurableExamined);
    }

    private void OnStationInit(StationGridAddedEvent ev)
    {
        var comp = EnsureComp<NavMapComponent>(ev.GridId);
<<<<<<< HEAD
        RefreshGrid(ev.GridId, comp, Comp<MapGridComponent>(ev.GridId));
    }

    #region: Grid change event handling

    private void OnNavMapSplit(ref GridSplitEvent args)
    {
        if (!_navQuery.TryComp(args.Grid, out var comp))
            return;
=======
        RefreshGrid(comp, Comp<MapGridComponent>(ev.GridId));
    }

    private void OnNavMapBeaconMapInit(EntityUid uid, NavMapBeaconComponent component, MapInitEvent args)
    {
        if (component.DefaultText == null || component.Text != null)
            return;

        component.Text = Loc.GetString(component.DefaultText);
        Dirty(uid, component);
        RefreshNavGrid(uid);
    }

    private void OnNavMapBeaconStartup(EntityUid uid, NavMapBeaconComponent component, ComponentStartup args)
    {
        RefreshNavGrid(uid);
    }

    private void OnNavMapBeaconAnchor(EntityUid uid, NavMapBeaconComponent component, ref AnchorStateChangedEvent args)
    {
        UpdateBeaconEnabledVisuals((uid, component));
        RefreshNavGrid(uid);
    }

    private void OnNavMapDoorStartup(Entity<NavMapDoorComponent> ent, ref ComponentStartup args)
    {
        RefreshNavGrid(ent);
    }

    private void OnNavMapDoorAnchor(Entity<NavMapDoorComponent> ent, ref AnchorStateChangedEvent args)
    {
        RefreshNavGrid(ent);
    }

    private void OnConfigureMessage(Entity<ConfigurableNavMapBeaconComponent> ent, ref NavMapBeaconConfigureBuiMessage args)
    {
        if (args.Session.AttachedEntity is not { } user)
            return;

        if (!TryComp<NavMapBeaconComponent>(ent, out var navMap))
            return;

        if (navMap.Text == args.Text &&
            navMap.Color == args.Color &&
            navMap.Enabled == args.Enabled)
            return;

        _adminLog.Add(LogType.Action, LogImpact.Medium,
            $"{ToPrettyString(user):player} configured NavMapBeacon \'{ToPrettyString(ent):entity}\' with text \'{args.Text}\', color {args.Color.ToHexNoAlpha()}, and {(args.Enabled ? "enabled" : "disabled")} it.");

        if (TryComp<WarpPointComponent>(ent, out var warpPoint))
        {
            warpPoint.Location = args.Text;
        }

        navMap.Text = args.Text;
        navMap.Color = args.Color;
        navMap.Enabled = args.Enabled;
        UpdateBeaconEnabledVisuals((ent, navMap));
        Dirty(ent, navMap);
        RefreshNavGrid(ent);
    }

    private void OnConfigurableMapInit(Entity<ConfigurableNavMapBeaconComponent> ent, ref MapInitEvent args)
    {
        if (!TryComp<NavMapBeaconComponent>(ent, out var navMap))
            return;

        // We set this on mapinit just in case the text was edited via VV or something.
        if (TryComp<WarpPointComponent>(ent, out var warpPoint))
        {
            warpPoint.Location = navMap.Text;
        }

        UpdateBeaconEnabledVisuals((ent, navMap));
    }

    private void OnConfigurableExamined(Entity<ConfigurableNavMapBeaconComponent> ent, ref ExaminedEvent args)
    {
        if (!args.IsInDetailsRange || !TryComp<NavMapBeaconComponent>(ent, out var navMap))
            return;

        args.PushMarkup(Loc.GetString("nav-beacon-examine-text",
            ("enabled", navMap.Enabled),
            ("color", navMap.Color.ToHexNoAlpha()),
            ("label", navMap.Text ?? string.Empty)));
    }

    private void UpdateBeaconEnabledVisuals(Entity<NavMapBeaconComponent> ent)
    {
        _appearance.SetData(ent, NavMapBeaconVisuals.Enabled, ent.Comp.Enabled && Transform(ent).Anchored);
    }

    /// <summary>
    /// Refreshes the grid for the corresponding beacon.
    /// </summary>
    /// <param name="uid"></param>
    private void RefreshNavGrid(EntityUid uid)
    {
        var xform = Transform(uid);

        if (!TryComp<NavMapComponent>(xform.GridUid, out var navMap))
            return;

        Dirty(xform.GridUid.Value, navMap);
    }

    private bool CanBeacon(EntityUid uid, TransformComponent? xform = null)
    {
        if (!Resolve(uid, ref xform))
            return false;

        return xform.GridUid != null && xform.Anchored;
    }

    private void OnNavMapStartup(EntityUid uid, NavMapComponent component, ComponentStartup args)
    {
        if (!TryComp<MapGridComponent>(uid, out var grid))
            return;

        RefreshGrid(component, grid);
    }

    private void OnNavMapSplit(ref GridSplitEvent args)
    {
        if (!TryComp(args.Grid, out NavMapComponent? comp))
            return;

        var gridQuery = GetEntityQuery<MapGridComponent>();
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

        foreach (var grid in args.NewGrids)
        {
            var newComp = EnsureComp<NavMapComponent>(grid);
<<<<<<< HEAD
            RefreshGrid(grid, newComp, _gridQuery.GetComponent(grid));
        }

        RefreshGrid(args.Grid, comp, _gridQuery.GetComponent(args.Grid));
    }

    private NavMapChunk EnsureChunk(NavMapComponent component, Vector2i origin)
=======
            RefreshGrid(newComp, gridQuery.GetComponent(grid));
        }

        RefreshGrid(comp, gridQuery.GetComponent(args.Grid));
    }

    private void RefreshGrid(NavMapComponent component, MapGridComponent grid)
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    {
        if (!component.Chunks.TryGetValue(origin, out var chunk))
        {
<<<<<<< HEAD
            chunk = new(origin);
            component.Chunks[origin] = chunk;
        }

        return chunk;
=======
            var chunkOrigin = SharedMapSystem.GetChunkIndices(tile.Value.GridIndices, ChunkSize);

            if (!component.Chunks.TryGetValue(chunkOrigin, out var chunk))
            {
                chunk = new NavMapChunk(chunkOrigin);
                component.Chunks[chunkOrigin] = chunk;
            }

            RefreshTile(grid, component, chunk, tile.Value.GridIndices);
        }
    }

    private void OnGetState(EntityUid uid, NavMapComponent component, ref ComponentGetState args)
    {
        if (!TryComp<MapGridComponent>(uid, out var mapGrid))
            return;

        var data = new Dictionary<Vector2i, int>(component.Chunks.Count);
        foreach (var (index, chunk) in component.Chunks)
        {
            data.Add(index, chunk.TileData);
        }

        var beaconQuery = AllEntityQuery<NavMapBeaconComponent, TransformComponent>();
        var beacons = new List<NavMapBeacon>();

        while (beaconQuery.MoveNext(out var beaconUid, out var beacon, out var xform))
        {
            if (!beacon.Enabled || xform.GridUid != uid || !CanBeacon(beaconUid, xform))
                continue;

            // TODO: Make warp points use metadata name instead.
            string? name = beacon.Text;

            if (string.IsNullOrEmpty(name))
            {
                if (TryComp<WarpPointComponent>(beaconUid, out var warpPoint) && warpPoint.Location != null)
                {
                    name = warpPoint.Location;
                }
                else
                {
                    name = MetaData(beaconUid).EntityName;
                }
            }

            beacons.Add(new NavMapBeacon(beacon.Color, name, xform.LocalPosition));
        }

        var airlockQuery = EntityQueryEnumerator<NavMapDoorComponent, TransformComponent>();
        var airlocks = new List<NavMapAirlock>();
        while (airlockQuery.MoveNext(out _, out _, out var xform))
        {
            if (xform.GridUid != uid || !xform.Anchored)
                continue;

            var pos = _map.TileIndicesFor(uid, mapGrid, xform.Coordinates);
            var enumerator = _map.GetAnchoredEntitiesEnumerator(uid, mapGrid, pos);

            var wallPresent = false;
            while (enumerator.MoveNext(out var ent))
            {
                if (!_physicsQuery.TryGetComponent(ent, out var body) ||
                    !body.CanCollide ||
                    !body.Hard ||
                    body.BodyType != BodyType.Static ||
                    !_tags.HasTag(ent.Value, "Wall", _tagQuery) &&
                    !_tags.HasTag(ent.Value, "Window", _tagQuery))
                {
                    continue;
                }

                wallPresent = true;
                break;
            }

            if (wallPresent)
                continue;

            airlocks.Add(new NavMapAirlock(xform.LocalPosition));
        }

        // TODO: Diffs
        args.State = new NavMapComponentState()
        {
            TileData = data,
            Beacons = beacons,
            Airlocks = airlocks
        };
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }

    private void OnTileChanged(ref TileChangedEvent ev)
    {
<<<<<<< HEAD
        if (!ev.EmptyChanged || !_navQuery.TryComp(ev.NewTile.GridUid, out var navMap))
=======
        if (TryComp<MapGridComponent>(ev.OldGrid, out var oldGrid) &&
            TryComp<NavMapComponent>(ev.OldGrid, out var navMap))
        {
            var chunkOrigin = SharedMapSystem.GetChunkIndices(ev.TilePos, ChunkSize);

            if (navMap.Chunks.TryGetValue(chunkOrigin, out var chunk))
            {
                RefreshTile(oldGrid, navMap, chunk, ev.TilePos);
            }
        }

        HandleAnchor(ev.Xform);
    }

    private void OnAnchorChange(ref AnchorStateChangedEvent ev)
    {
        HandleAnchor(ev.Transform);
    }

    private void HandleAnchor(TransformComponent xform)
    {
        if (!TryComp<NavMapComponent>(xform.GridUid, out var navMap) ||
            !TryComp<MapGridComponent>(xform.GridUid, out var grid))
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            return;

        var tile = ev.NewTile.GridIndices;
        var chunkOrigin = SharedMapSystem.GetChunkIndices(tile, ChunkSize);

        var chunk = EnsureChunk(navMap, chunkOrigin);

        // This could be easily replaced in the future to accommodate diagonal tiles
        var relative = SharedMapSystem.GetChunkRelative(tile, ChunkSize);
        ref var tileData = ref chunk.TileData[GetTileIndex(relative)];

        if (ev.NewTile.IsSpace(_tileDefManager))
        {
            tileData = 0;
            if (PruneEmpty((ev.NewTile.GridUid, navMap), chunk))
                return;
        }
        else
        {
            tileData = FloorMask;
        }

<<<<<<< HEAD
        DirtyChunk((ev.NewTile.GridUid, navMap), chunk);
    }

    private void DirtyChunk(Entity<NavMapComponent> entity, NavMapChunk chunk)
    {
        if (chunk.LastUpdate == _gameTiming.CurTick)
            return;

        chunk.LastUpdate = _gameTiming.CurTick;
        Dirty(entity);
    }

    private void OnAirtightChange(ref AirtightChanged args)
    {
        if (args.AirBlockedChanged)
            return;

        var gridUid = args.Position.Grid;

        if (!_navQuery.TryComp(gridUid, out var navMap) ||
            !_gridQuery.TryComp(gridUid, out var mapGrid))
        {
            return;
        }

        var chunkOrigin = SharedMapSystem.GetChunkIndices(args.Position.Tile, ChunkSize);
        var (newValue, chunk) = RefreshTileEntityContents(gridUid, navMap, mapGrid, chunkOrigin, args.Position.Tile, setFloor: false);

        if (newValue == 0 && PruneEmpty((gridUid, navMap), chunk))
            return;

        DirtyChunk((gridUid, navMap), chunk);
    }

    #endregion

    #region: Beacon event handling

    private void OnNavMapBeaconMapInit(EntityUid uid, NavMapBeaconComponent component, MapInitEvent args)
    {
        if (component.DefaultText == null || component.Text != null)
            return;

        component.Text = Loc.GetString(component.DefaultText);
        Dirty(uid, component);

        UpdateNavMapBeaconData(uid, component);
    }

    private void OnNavMapBeaconAnchor(EntityUid uid, NavMapBeaconComponent component, ref AnchorStateChangedEvent args)
    {
        UpdateBeaconEnabledVisuals((uid, component));
        UpdateNavMapBeaconData(uid, component);
    }

    private void OnConfigureMessage(Entity<ConfigurableNavMapBeaconComponent> ent, ref NavMapBeaconConfigureBuiMessage args)
    {
        if (!TryComp<NavMapBeaconComponent>(ent, out var beacon))
            return;

        if (beacon.Text == args.Text &&
            beacon.Color == args.Color &&
            beacon.Enabled == args.Enabled)
            return;

        _adminLog.Add(LogType.Action, LogImpact.Medium,
            $"{ToPrettyString(args.Actor):player} configured NavMapBeacon \'{ToPrettyString(ent):entity}\' with text \'{args.Text}\', color {args.Color.ToHexNoAlpha()}, and {(args.Enabled ? "enabled" : "disabled")} it.");

        if (TryComp<WarpPointComponent>(ent, out var warpPoint))
        {
            warpPoint.Location = args.Text;
        }

        beacon.Text = args.Text;
        beacon.Color = args.Color;
        beacon.Enabled = args.Enabled;

        UpdateBeaconEnabledVisuals((ent, beacon));
        UpdateNavMapBeaconData(ent, beacon);
    }

    private void OnConfigurableMapInit(Entity<ConfigurableNavMapBeaconComponent> ent, ref MapInitEvent args)
    {
        if (!TryComp<NavMapBeaconComponent>(ent, out var navMap))
            return;

        // We set this on mapinit just in case the text was edited via VV or something.
        if (TryComp<WarpPointComponent>(ent, out var warpPoint))
            warpPoint.Location = navMap.Text;

        UpdateBeaconEnabledVisuals((ent, navMap));
    }

    private void OnConfigurableExamined(Entity<ConfigurableNavMapBeaconComponent> ent, ref ExaminedEvent args)
    {
        if (!args.IsInDetailsRange || !TryComp<NavMapBeaconComponent>(ent, out var navMap))
            return;

        args.PushMarkup(Loc.GetString("nav-beacon-examine-text",
            ("enabled", navMap.Enabled),
            ("color", navMap.Color.ToHexNoAlpha()),
            ("label", navMap.Text ?? string.Empty)));
    }

    #endregion

    #region: Grid functions

    private void RefreshGrid(EntityUid uid, NavMapComponent component, MapGridComponent mapGrid)
    {
        // Clear stale data
        component.Chunks.Clear();
        component.Beacons.Clear();

        // Loop over all tiles
        var tileRefs = _mapSystem.GetAllTiles(uid, mapGrid);

        foreach (var tileRef in tileRefs)
        {
            var tile = tileRef.GridIndices;
            var chunkOrigin = SharedMapSystem.GetChunkIndices(tile, ChunkSize);

            var chunk = EnsureChunk(component, chunkOrigin);
            chunk.LastUpdate = _gameTiming.CurTick;
            RefreshTileEntityContents(uid, component, mapGrid, chunkOrigin, tile, setFloor: true);
        }

        Dirty(uid, component);
    }

    private (int NewVal, NavMapChunk Chunk) RefreshTileEntityContents(EntityUid uid,
        NavMapComponent component,
        MapGridComponent mapGrid,
        Vector2i chunkOrigin,
        Vector2i tile,
        bool setFloor)
    {
        var relative = SharedMapSystem.GetChunkRelative(tile, ChunkSize);
        var chunk = EnsureChunk(component, chunkOrigin);
        ref var tileData = ref chunk.TileData[GetTileIndex(relative)];

        // Clear all data except for floor bits
        if (setFloor)
            tileData = FloorMask;
        else
            tileData &= FloorMask;
=======
        RefreshTile(grid, navMap, chunk, tile);
    }

    private void RefreshTile(MapGridComponent grid, NavMapComponent component, NavMapChunk chunk, Vector2i tile)
    {
        var relative = SharedMapSystem.GetChunkRelative(tile, ChunkSize);
        var existing = chunk.TileData;
        var flag = GetFlag(relative);

        chunk.TileData &= ~flag;

        var enumerator = grid.GetAnchoredEntitiesEnumerator(tile);
        // TODO: Use something to get convex poly.
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

        var enumerator = _mapSystem.GetAnchoredEntitiesEnumerator(uid, mapGrid, tile);
        while (enumerator.MoveNext(out var ent))
        {
<<<<<<< HEAD
            if (!_airtightQuery.TryComp(ent, out var airtight))
=======
            if (!_physicsQuery.TryGetComponent(ent, out var body) ||
                !body.CanCollide ||
                !body.Hard ||
                body.BodyType != BodyType.Static ||
                !_tags.HasTag(ent.Value, "Wall", _tagQuery) &&
                !_tags.HasTag(ent.Value, "Window", _tagQuery))
            {
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
                continue;

            var category = GetEntityType(ent.Value);
            if (category == NavMapChunkType.Invalid)
                continue;

            var directions = (int)airtight.AirBlockedDirection;
            tileData |= directions << (int) category;
        }

        // Remove walls that intersect with doors (unless they can both physically fit on the same tile)
        // TODO NAVMAP why can this even happen?
        // Is this for blast-doors or something?

        // Shift airlock bits over to the wall bits
        var shiftedAirlockBits = (tileData & AirlockMask) >> ((int) NavMapChunkType.Airlock - (int) NavMapChunkType.Wall);

        // And then mask door bits
        tileData &= ~shiftedAirlockBits;

        return (tileData, chunk);
    }

    private bool PruneEmpty(Entity<NavMapComponent> entity, NavMapChunk chunk)
    {
        foreach (var val in chunk.TileData)
        {
            // TODO NAVMAP SIMD
            if (val != 0)
                return false;
        }

        entity.Comp.Chunks.Remove(chunk.Origin);
        Dirty(entity);
        return true;
    }

    #endregion

    #region: Beacon functions

    private void UpdateNavMapBeaconData(EntityUid uid, NavMapBeaconComponent component, TransformComponent? xform = null)
    {
        if (!Resolve(uid, ref xform))
            return;

        if (xform.GridUid == null)
            return;

        if (!_navQuery.TryComp(xform.GridUid, out var navMap))
            return;

        var meta = MetaData(uid);
        var changed = navMap.Beacons.Remove(meta.NetEntity);

        if (TryCreateNavMapBeaconData(uid, component, xform, meta, out var beaconData))
        {
            navMap.Beacons.Add(meta.NetEntity, beaconData.Value);
            changed = true;
        }

        if (changed)
            Dirty(xform.GridUid.Value, navMap);
    }

<<<<<<< HEAD
    private void UpdateBeaconEnabledVisuals(Entity<NavMapBeaconComponent> ent)
    {
        _appearance.SetData(ent, NavMapBeaconVisuals.Enabled, ent.Comp.Enabled && Transform(ent).Anchored);
    }

=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    /// <summary>
    /// Sets the beacon's Enabled field and refreshes the grid.
    /// </summary>
    public void SetBeaconEnabled(EntityUid uid, bool enabled, NavMapBeaconComponent? comp = null)
    {
        if (!Resolve(uid, ref comp) || comp.Enabled == enabled)
            return;

        comp.Enabled = enabled;
        UpdateBeaconEnabledVisuals((uid, comp));
<<<<<<< HEAD
=======
        Dirty(uid, comp);

        RefreshNavGrid(uid);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }

    /// <summary>
    /// Toggles the beacon's Enabled field and refreshes the grid.
    /// </summary>
    public void ToggleBeacon(EntityUid uid, NavMapBeaconComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        SetBeaconEnabled(uid, !comp.Enabled, comp);
    }

    /// <summary>
    /// For a given position, tries to find the nearest configurable beacon that is marked as visible.
    /// This is used for things like announcements where you want to find the closest "landmark" to something.
    /// </summary>
    [PublicAPI]
    public bool TryGetNearestBeacon(Entity<TransformComponent?> ent,
        [NotNullWhen(true)] out Entity<NavMapBeaconComponent>? beacon,
        [NotNullWhen(true)] out MapCoordinates? beaconCoords)
    {
        beacon = null;
        beaconCoords = null;
        if (!Resolve(ent, ref ent.Comp))
            return false;

<<<<<<< HEAD
        return TryGetNearestBeacon(_transformSystem.GetMapCoordinates(ent, ent.Comp), out beacon, out beaconCoords);
=======
        return TryGetNearestBeacon(_transform.GetMapCoordinates(ent, ent.Comp), out beacon, out beaconCoords);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }

    /// <summary>
    /// For a given position, tries to find the nearest configurable beacon that is marked as visible.
    /// This is used for things like announcements where you want to find the closest "landmark" to something.
    /// </summary>
    public bool TryGetNearestBeacon(MapCoordinates coordinates,
        [NotNullWhen(true)] out Entity<NavMapBeaconComponent>? beacon,
        [NotNullWhen(true)] out MapCoordinates? beaconCoords)
    {
        beacon = null;
        beaconCoords = null;
        var minDistance = float.PositiveInfinity;

        var query = EntityQueryEnumerator<ConfigurableNavMapBeaconComponent, NavMapBeaconComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out _, out var navBeacon, out var xform))
        {
            if (!navBeacon.Enabled)
                continue;

            if (navBeacon.Text == null)
                continue;

            if (coordinates.MapId != xform.MapID)
                continue;

<<<<<<< HEAD
            var coords = _transformSystem.GetWorldPosition(xform);
=======
            var coords = _transform.GetWorldPosition(xform);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            var distanceSquared = (coordinates.Position - coords).LengthSquared();
            if (!float.IsInfinity(minDistance) && distanceSquared >= minDistance)
                continue;

            minDistance = distanceSquared;
            beacon = (uid, navBeacon);
            beaconCoords = new MapCoordinates(coords, xform.MapID);
        }

        return beacon != null;
    }

    [PublicAPI]
    public string GetNearestBeaconString(Entity<TransformComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp))
            return Loc.GetString("nav-beacon-pos-no-beacons");

<<<<<<< HEAD
        return GetNearestBeaconString(_transformSystem.GetMapCoordinates(ent, ent.Comp));
=======
        return GetNearestBeaconString(_transform.GetMapCoordinates(ent, ent.Comp));
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }

    public string GetNearestBeaconString(MapCoordinates coordinates)
    {
        if (!TryGetNearestBeacon(coordinates, out var beacon, out var pos))
            return Loc.GetString("nav-beacon-pos-no-beacons");

        var gridOffset = Angle.Zero;
        if (_mapManager.TryFindGridAt(pos.Value, out var grid, out _))
            gridOffset = Transform(grid).LocalRotation;

        // get the angle between the two positions, adjusted for the grid rotation so that
        // we properly preserve north in relation to the grid.
        var dir = (pos.Value.Position - coordinates.Position).ToWorldAngle();
        var adjustedDir = (dir - gridOffset).GetDir();

        var length = (pos.Value.Position - coordinates.Position).Length();
        if (length < CloseDistance)
        {
            return Loc.GetString("nav-beacon-pos-format",
                ("color", beacon.Value.Comp.Color),
                ("marker", beacon.Value.Comp.Text!));
        }

        var modifier = length > FarDistance
            ? Loc.GetString("nav-beacon-pos-format-direction-mod-far")
            : string.Empty;

<<<<<<< HEAD
        // we can null suppress the text being null because TryGetNearestVisibleStationBeacon always gives us a beacon with not-null text.
=======
        // we can null suppress the text being null because TRyGetNearestVisibleStationBeacon always gives us a beacon with not-null text.
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        return Loc.GetString("nav-beacon-pos-format-direction",
            ("modifier", modifier),
            ("direction", ContentLocalizationManager.FormatDirection(adjustedDir).ToLowerInvariant()),
            ("color", beacon.Value.Comp.Color),
            ("marker", beacon.Value.Comp.Text!));
    }
<<<<<<< HEAD

    #endregion
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
}
