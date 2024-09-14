using Content.Shared.Tools;
<<<<<<< HEAD
=======
using Content.Shared.Storage;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
using Robust.Shared.Prototypes;

namespace Content.Server.Construction.Components;

/// <summary>
/// Used for something that can be refined by welder.
/// For example, glass shard can be refined to glass sheet.
/// </summary>
[RegisterComponent]
public sealed partial class WelderRefinableComponent : Component
{
<<<<<<< HEAD
    [DataField]
    public HashSet<EntProtoId>? RefineResult = new();

    [DataField]
    public float RefineTime = 2f;

    [DataField]
    public float RefineFuel;

    [DataField]
    public ProtoId<ToolQualityPrototype> QualityNeeded = "Welding";
=======
    /// <summary>
    /// Used for something that can be refined by welder.
    /// For example, glass shard can be refined to glass sheet.
    /// </summary>
    [RegisterComponent]
    public sealed partial class WelderRefinableComponent : Component
    {
        [DataField]
        public List<EntitySpawnEntry> RefineResult = new();

        [DataField]
        public float RefineTime = 2f;

        [DataField]
        public ProtoId<ToolQualityPrototype> QualityNeeded = "Welding";
    }
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
}
