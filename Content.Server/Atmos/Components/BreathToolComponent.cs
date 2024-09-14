using Content.Shared.Inventory;

namespace Content.Server.Atmos.Components
{
    /// <summary>
    /// Used in internals as breath tool.
    /// </summary>
    [RegisterComponent]
    [ComponentProtoName("BreathMask")]
    public sealed partial class BreathToolComponent : Component
    {
        /// <summary>
        /// Tool is functional only in allowed slots
        /// </summary>
<<<<<<< HEAD
        [DataField]
=======
        [DataField("allowedSlots")]
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        public SlotFlags AllowedSlots = SlotFlags.MASK | SlotFlags.HEAD;
        public bool IsFunctional;

        public EntityUid? ConnectedInternalsEntity;
    }
}
