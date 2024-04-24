namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;

public sealed partial class SpawnEntity : SlimeReagentEffect
{
    [DataField("toSpawn")]
    public string ToSpawn;

    [DataField("amount")]
    public int Amount;

    public override void Effect(SlimeReagentEffectArgs args)
    {
        // Com os args, que deve ser algo tipo target e outras coisas, fazer o codigo funcionar por aqui, pra nao precisar definir tudo no
        // system de SlimeReaction. Quero apenas chamar em slimeReaction o SlimeReagentEffect.Effect() pra funcionar automatico plss
        throw new NotImplementedException();
    }
}
