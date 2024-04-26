using Content.Shared.DoAfter;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.Serialization;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeGrinder;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class SlimeGrinderComponent : Component
{
    [DataField("grindTimeMultiplier"), ViewVariables(VVAccess.ReadWrite)]
    public float GrindTimeMultiplier = 1;

    #region  audio
    [DataField("clickSound")]
    public SoundSpecifier ClickSound = new SoundPathSpecifier("/Audio/Machines/machine_switch.ogg");

    public EntityUid? PlayingStream;

    [DataField("loopingSound")]
    public SoundSpecifier LoopingSound = new SoundPathSpecifier("/Audio/Ambience/Objects/engine_hum.ogg");
    #endregion

    [ViewVariables]
    public Container Storage = default!;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int Capacity = 10;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float BaseInsertionDelay = 1f;

    /// <summary>
    /// This gets set for each mob it processes.
    /// When it hits 0, spit out biomass.
    /// </summary>
    [ViewVariables]
    public float ProcessingTimer = 2f;

    public float ProcessingTimerTotal = 2f;
}

public sealed class BeingSlimeGrindedEvent : HandledEntityEventArgs
{
    public EntityUid Grinder;
    public EntityUid? User;

    public BeingSlimeGrindedEvent(EntityUid grinder, EntityUid? user)
    {
        Grinder = grinder;
        User = user;
    }
}

[Serializable, NetSerializable]
public sealed partial class SlimeGrinderDoAfterEvent : SimpleDoAfterEvent
{
}
