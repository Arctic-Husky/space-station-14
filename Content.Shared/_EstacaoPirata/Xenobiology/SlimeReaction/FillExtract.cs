namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;

public sealed partial class FillExtract : SlimeReagentEffect
{

    [DataField("reagent")]
    public string Reagent;

    [DataField("quantity")]
    public float Quantity;

    [DataField("minReagentRequired")]
    public float MinReagentRequired;

    public override void Effect(SlimeReagentEffectArgs args)
    {
        throw new NotImplementedException();
    }
}
