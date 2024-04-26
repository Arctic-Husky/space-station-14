using Robust.Shared.Audio;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeGrinder;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class ActiveSlimeGrinderComponent : Component
{
    [DataField("finishingSound")]
    public SoundSpecifier FinishingSound = new SoundPathSpecifier("/Audio/EstacaoPirata/Machines/engine_hum_finish.ogg");
}
