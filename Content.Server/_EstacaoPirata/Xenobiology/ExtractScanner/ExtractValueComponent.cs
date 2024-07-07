namespace Content.Server._EstacaoPirata.Xenobiology.ExtractScanner;

/// <summary>
/// This is used for holding the information about the slime extract value and tier number
/// </summary>
[RegisterComponent]
public sealed partial class ExtractValueComponent : Component
{
    [DataField("tier"), ViewVariables]
    public byte Tier = 0;  // Tirar tier 0 e fazer a partir do 1 ?

    [ViewVariables]
    public int Value = 0; // This is not being used by the system, its just for debugging for now

    [DataField("base")]
    public int BaseValue = 250;

    [DataField("decrease")]
    public int SpentDecrease = 2;
}
