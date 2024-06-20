using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class ActiveSlimeReactionComponent : Component
{
    public Dictionary<SlimeReagentEffect, SlimeReagentEffectArgs> Effects = new Dictionary<SlimeReagentEffect, SlimeReagentEffectArgs>();

    public float WaitTime = 0f;

    public float MaxWaitTimeToDeactivate = 5f;

    public float WaitTimeToDeactivate = 5f;

    public bool ReactionSuccess = false;

    public bool SpendOnUse = false;

    public bool SoundPlayed = false;
}
