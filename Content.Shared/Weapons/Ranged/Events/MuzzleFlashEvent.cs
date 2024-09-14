using Robust.Shared.Serialization;

namespace Content.Shared.Weapons.Ranged.Events;

/// <summary>
/// Raised whenever a muzzle flash client-side entity needs to be spawned.
/// </summary>
[Serializable, NetSerializable]
public sealed class MuzzleFlashEvent : EntityEventArgs
{
    public NetEntity Uid;
    public string Prototype;

    public Angle Angle;

<<<<<<< HEAD
    public MuzzleFlashEvent(NetEntity uid, string prototype, Angle angle)
=======
    public MuzzleFlashEvent(NetEntity uid, string prototype, bool matchRotation = false)
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    {
        Uid = uid;
        Prototype = prototype;
        Angle = angle;
    }
}
