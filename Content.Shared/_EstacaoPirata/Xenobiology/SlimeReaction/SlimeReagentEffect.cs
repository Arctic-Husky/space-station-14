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
    // TODO criar um pre effect e deixar o effect pra funcionar 100% do tempo se chamado

    public virtual bool Effect(SlimeReagentEffectArgs args)
    {
        return false;
    }

    public virtual bool TryEffect(SlimeReagentEffectArgs args)
    {
        return false;
    }

    public virtual float GetTimeNeeded()
    {
        return 0f;
    }

    public virtual bool GetSpendOnUse()
    {
        return true;
    }

    public virtual string GetReactionMessage()
    {
        return "extract-reacting";
    }
}

// TODO para o rework, isso aqui vai ter que mudar. Quero fazer com que seja possivel interação por toque também, para permitir crossbreeding em slimes com 10 extracts

// TODO rework
public readonly record struct SlimeReagentEffectArgs(
    string Prototype,
    EntityUid ExtractEntity,
    FixedPoint2 Quantity,
    IEntityManager EntityManager,
    IRobustRandom RobustRandom,
    SlimeReactionComponent ReactionComponent,
    SoundSpecifier? Sound
);
