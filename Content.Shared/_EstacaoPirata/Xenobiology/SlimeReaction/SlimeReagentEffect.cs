using JetBrains.Annotations;
using Robust.Shared.Random;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;

[ImplicitDataDefinitionForInheritors]
[MeansImplicitUse]
public abstract partial class SlimeReagentEffect
{
    public abstract bool Effect(SlimeReagentEffectArgs args);
}

// Se os nomes aqui estiverem iguais aos reagentes, da pra usar o ToString() pras coisas
public enum SlimeReactionMethod
{
    Plasma,
    Blood,
    Water
}

public readonly record struct SlimeReagentEffectArgs(
    string Prototype,
    EntityUid? ExtractEntity,
    float Quantity,
    IEntityManager EntityManager,
    IRobustRandom RobustRandom
);
