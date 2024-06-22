namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeGrinder;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class SlimeCoreComponent : Component
{
    [DataField("core")]
    public string Core = "GraySlimeExtract";

    [DataField("yield")]
    public float Yield = 1f;
}
