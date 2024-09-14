using Content.Shared.Inventory;

namespace Content.Shared.Electrocution
{
    public sealed class ElectrocutionAttemptEvent : CancellableEntityEventArgs, IInventoryRelayEvent
    {
        public SlotFlags TargetSlots { get; }

        public readonly EntityUid TargetUid;
        public readonly EntityUid? SourceUid;
        public float SiemensCoefficient = 1f;

        public ElectrocutionAttemptEvent(EntityUid targetUid, EntityUid? sourceUid, float siemensCoefficient, SlotFlags targetSlots)
        {
            TargetUid = targetUid;
            TargetSlots = targetSlots;
            SourceUid = sourceUid;
            SiemensCoefficient = siemensCoefficient;
        }
    }

    public sealed class ElectrocutedEvent : EntityEventArgs
    {
        public readonly EntityUid TargetUid;
        public readonly EntityUid? SourceUid;
        public readonly float SiemensCoefficient;
<<<<<<< HEAD
        public readonly float? ShockDamage = null; // Parkstation-IPC

        public ElectrocutedEvent(EntityUid targetUid, EntityUid? sourceUid, float siemensCoefficient, float shockDamage) // Parkstation-IPC
=======
        public readonly float? ShockDamage = null; 

        public ElectrocutedEvent(EntityUid targetUid, EntityUid? sourceUid, float siemensCoefficient, float shockDamage) 
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        {
            TargetUid = targetUid;
            SourceUid = sourceUid;
            SiemensCoefficient = siemensCoefficient;
<<<<<<< HEAD
            ShockDamage = shockDamage; // Parkstation-IPC
=======
            ShockDamage = shockDamage; 
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        }
    }
}
