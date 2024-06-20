using Content.Shared.FixedPoint;
using JetBrains.Annotations;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;

[ImplicitDataDefinitionForInheritors]
[MeansImplicitUse]
[Virtual]
public partial class SlimeReagentEffect
{
    public virtual bool Effect(SlimeReagentEffectArgs args)
    {
        return false;
    }

    public virtual float TimeNeeded()
    {
        return 0f;
    }

    public virtual bool SpendOnUse()
    {
        return true;
    }

    public virtual string GetReactionMessage()
    {
        return "extract-reacting";
    }
}

// Se os nomes aqui estiverem iguais aos reagentes, da pra usar o ToString() pras coisas
public enum SlimeReactionMethod
{
    Plasma,
    Blood,
    Water,
    Sugar,
    Radium,
    ExtractEnhancer,
}

public readonly record struct SlimeReagentEffectArgs(
    string Prototype,
    EntityUid ExtractEntity,
    FixedPoint2 Quantity,
    IEntityManager EntityManager,
    IRobustRandom RobustRandom,
    SlimeReactionComponent ReactionComponent,
    SoundSpecifier? Sound
);
