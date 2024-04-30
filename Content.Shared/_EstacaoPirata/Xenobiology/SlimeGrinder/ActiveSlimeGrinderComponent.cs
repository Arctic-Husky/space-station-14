using Robust.Shared.Audio;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeGrinder;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class ActiveSlimeGrinderComponent : Component
{
    [DataField("startingSound")]
    public SoundSpecifier StartingSound = new SoundPathSpecifier("/Audio/Machines/reclaimer_startup.ogg");
}
