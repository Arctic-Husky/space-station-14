namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeGrinder;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class SlimeGrindableComponent : Component
{
    [DataField("grindResult")]
    public string GrindResult = "GraySlimeExtract";
}
