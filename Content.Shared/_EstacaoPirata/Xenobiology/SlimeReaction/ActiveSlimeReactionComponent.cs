namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class ActiveSlimeReactionComponent : Component
{
    public Dictionary<SlimeReagentEffect, SlimeReagentEffectArgs> Effects = new Dictionary<SlimeReagentEffect, SlimeReagentEffectArgs>();

    public float WaitTime = 0f;
}
