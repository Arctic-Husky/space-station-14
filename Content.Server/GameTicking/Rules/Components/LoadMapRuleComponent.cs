using Content.Server.Maps;
<<<<<<< HEAD
using Content.Shared.GridPreloader.Prototypes;
using Content.Shared.Storage;
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
using Content.Shared.Whitelist;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.GameTicking.Rules.Components;

/// <summary>
/// This is used for a game rule that loads a map when activated.
/// </summary>
[RegisterComponent]
public sealed partial class LoadMapRuleComponent : Component
{
    [DataField]
    public MapId? Map;

    [DataField]
<<<<<<< HEAD
    public ProtoId<GameMapPrototype>? GameMap;
=======
    public ProtoId<GameMapPrototype>? GameMap ;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    [DataField]
    public ResPath? MapPath;

    [DataField]
<<<<<<< HEAD
    public ProtoId<PreloadedGridPrototype>? PreloadedGrid;

    [DataField]
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    public List<EntityUid> MapGrids = new();

    [DataField]
    public EntityWhitelist? SpawnerWhitelist;
}
