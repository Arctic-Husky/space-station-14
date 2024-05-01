using Content.Shared._EstacaoPirata.Xenobiology.Meiosis;
using Robust.Shared.Prototypes;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeGrowth;

/// <summary>
/// This is used for controlling the growth of a baby slime to adulthood
/// </summary>
[RegisterComponent]
public sealed partial class SlimeGrowthComponent : Component
{
    /// <summary>
    /// This entity's adult version
    /// </summary>
    [DataField("adult"), ViewVariables]
    public ProtoId<EntityPrototype> Adult;

    [ViewVariables]
    public MeiosisThreshold InheritedMutation = MeiosisThreshold.Mid;
}
