namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeFeeding;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class SlimeFoodComponent : Component
{
    [DataField("remaining"), ViewVariables]
    public float Remaining = 100f;
}
