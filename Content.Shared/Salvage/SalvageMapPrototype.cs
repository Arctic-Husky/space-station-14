using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Salvage;

[Prototype]
<<<<<<< HEAD
public sealed partial class SalvageMapPrototype : IPrototype
=======
public sealed class SalvageMapPrototype : IPrototype
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
{
    [ViewVariables] [IdDataField] public string ID { get; } = default!;

    /// <summary>
    /// Relative directory path to the given map, i.e. `Maps/Salvage/template.yml`
    /// </summary>
    [DataField(required: true)] public ResPath MapPath;
<<<<<<< HEAD
=======

    /// <summary>
    /// DeltaV - Used for getting the proper name for the map
    /// </summary>
    [DataField] public string Size { get; } = "unknown";
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
}
