using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Shared.SprayPainter.Prototypes;

/// <summary>
/// Maps airlock style names to department ids.
/// </summary>
[Prototype("airlockDepartments")]
<<<<<<< HEAD
public sealed partial class AirlockDepartmentsPrototype : IPrototype
=======
public sealed class AirlockDepartmentsPrototype : IPrototype
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// Dictionary of style names to department ids.
    /// If a style does not have a department (e.g. external) it is set to null.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<string, ProtoId<DepartmentPrototype>> Departments = new();
}
