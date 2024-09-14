using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Timing;

/// <summary>
<<<<<<< HEAD
/// Timer that creates a cooldown each time an object is activated/used.
/// Can support additional, separate cooldown timers on the object by passing a unique ID with the system methods.
/// </summary>
[RegisterComponent]
[NetworkedComponent]
=======
/// Timer that creates a cooldown each time an object is activated/used
/// </summary>
/// <remarks>
/// Currently it only supports a single delay per entity, this means that for things that have two delay interactions they will share one timer, so this can cause issues. For example, the bible has a delay when opening the storage UI and when applying it's interaction effect, and they share the same delay.
/// </remarks>
[RegisterComponent]
[NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause]
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
[Access(typeof(UseDelaySystem))]
public sealed partial class UseDelayComponent : Component
{
    [DataField]
    public Dictionary<string, UseDelayInfo> Delays = [];

    /// <summary>
<<<<<<< HEAD
    /// Default delay time.
    /// </summary>
    /// <remarks>
    /// This is only used at MapInit and should not be expected
    /// to reflect the length of the default delay after that.
    /// Use <see cref="UseDelaySystem.TryGetDelayInfo"/> instead.
    /// </remarks>
    [DataField]
    public TimeSpan Delay = TimeSpan.FromSeconds(1);
}

[Serializable, NetSerializable]
public sealed class UseDelayComponentState : IComponentState
{
    public Dictionary<string, UseDelayInfo> Delays = new();
}

[Serializable, NetSerializable]
[DataDefinition]
public sealed partial class UseDelayInfo
{
    [DataField]
    public TimeSpan Length { get; set; }
    [DataField]
    public TimeSpan StartTime { get; set; }
    [DataField]
    public TimeSpan EndTime { get; set; }

    public UseDelayInfo(TimeSpan length, TimeSpan startTime = default, TimeSpan endTime = default)
    {
        Length = length;
        StartTime = startTime;
        EndTime = endTime;
    }
=======
    /// When the delay starts.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan DelayStartTime;

    /// <summary>
    /// When the delay ends.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan DelayEndTime;

    /// <summary>
    /// Default delay time
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public TimeSpan Delay = TimeSpan.FromSeconds(1);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
}
