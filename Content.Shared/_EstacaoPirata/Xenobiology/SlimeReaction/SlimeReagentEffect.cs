using Content.Shared.FixedPoint;
using JetBrains.Annotations;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;

[ImplicitDataDefinitionForInheritors]
[MeansImplicitUse]
public abstract partial class SlimeReagentEffect
{
    public abstract bool Effect(SlimeReagentEffectArgs args);

    public abstract float NeedsTime();

    public abstract void PlaySound(SharedAudioSystem audioSystem, SoundSpecifier? sound, EntityUid entity);
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
    EntityUid ExtractEntity,
    FixedPoint2 Quantity,
    IEntityManager EntityManager,
    IRobustRandom RobustRandom,
    SlimeReactionComponent ReactionComponent
);
