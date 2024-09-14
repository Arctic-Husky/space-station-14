using Content.Server.UserInterface;
using Content.Shared.Instruments;
using Robust.Shared.Player;
<<<<<<< HEAD
using ActivatableUIComponent = Content.Shared.UserInterface.ActivatableUIComponent;
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

namespace Content.Server.Instruments;

[RegisterComponent]
public sealed partial class InstrumentComponent : SharedInstrumentComponent
{
    [Dependency] private readonly IEntityManager _entMan = default!;

    [ViewVariables] public float Timer = 0f;
    [ViewVariables] public int BatchesDropped = 0;
    [ViewVariables] public int LaggedBatches = 0;
    [ViewVariables] public int MidiEventCount = 0;
    [ViewVariables] public uint LastSequencerTick = 0;

    // TODO Instruments: Make this ECS
<<<<<<< HEAD
    public EntityUid? InstrumentPlayer =>
        _entMan.GetComponentOrNull<ActivatableUIComponent>(Owner)?.CurrentSingleUser
        ?? _entMan.GetComponentOrNull<ActorComponent>(Owner)?.PlayerSession.AttachedEntity;
=======
    public ICommonSession? InstrumentPlayer =>
        _entMan.GetComponentOrNull<ActivatableUIComponent>(Owner)?.CurrentSingleUser
        ?? _entMan.GetComponentOrNull<ActorComponent>(Owner)?.PlayerSession;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
}

[RegisterComponent]
public sealed partial class ActiveInstrumentComponent : Component
{
}
