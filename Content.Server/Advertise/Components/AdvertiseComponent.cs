<<<<<<< HEAD
using Content.Server.Advertise.EntitySystems;
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
using Content.Shared.Advertise;
using Robust.Shared.Prototypes;

namespace Content.Server.Advertise.Components;

/// <summary>
/// Makes this entity periodically advertise by speaking a randomly selected
/// message from a specified MessagePack into local chat.
/// </summary>
<<<<<<< HEAD
[RegisterComponent, Access(typeof(AdvertiseSystem))]
=======
[RegisterComponent]
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
public sealed partial class AdvertiseComponent : Component
{
    /// <summary>
    /// Minimum time in seconds to wait before saying a new ad, in seconds. Has to be larger than or equal to 1.
    /// </summary>
    [DataField]
<<<<<<< HEAD
    public int MinimumWait { get; private set; } = 8 * 60;
=======
    public int MinimumWait { get; set; } = 8 * 60;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    /// <summary>
    /// Maximum time in seconds to wait before saying a new ad, in seconds. Has to be larger than or equal
    /// to <see cref="MinimumWait"/>
    /// </summary>
    [DataField]
<<<<<<< HEAD
    public int MaximumWait { get; private set; } = 10 * 60;
=======
    public int MaximumWait { get; set; } = 10 * 60;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    /// <summary>
    /// If true, the delay before the first advertisement (at MapInit) will ignore <see cref="MinimumWait"/>
    /// and instead be rolled between 0 and <see cref="MaximumWait"/>. This only applies to the initial delay;
    /// <see cref="MinimumWait"/> will be respected after that.
    /// </summary>
    [DataField]
    public bool Prewarm = true;

    /// <summary>
    /// The identifier for the advertisements pack prototype.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<MessagePackPrototype> Pack { get; private set; }

    /// <summary>
    /// The next time an advertisement will be said.
    /// </summary>
    [DataField]
    public TimeSpan NextAdvertisementTime { get; set; } = TimeSpan.Zero;

}
