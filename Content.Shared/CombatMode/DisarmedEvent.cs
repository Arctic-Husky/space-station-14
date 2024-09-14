namespace Content.Shared.CombatMode
{
    public sealed class DisarmedEvent : HandledEntityEventArgs
    {
        /// <summary>
        ///     The entity being disarmed.
        /// </summary>
        public EntityUid Target { get; init; }

        /// <summary>
        ///     The entity performing the disarm.
        /// </summary>
        public EntityUid Source { get; init; }

        /// <summary>
        ///     Probability for push/knockdown.
        /// </summary>
        public float PushProbability { get; init; }

        /// <summary>
<<<<<<< HEAD
        ///     Prefix for the popup message that will be displayed on a successful push.
        ///     Should be set before returning.
        /// </summary>
        public string PopupPrefix { get; set; } = "";

        /// <summary>
        ///     Whether the entity was successfully stunned from a shove.
        /// </summary>
        public bool IsStunned { get; set; }
=======
        ///     Potential stamina damage if this disarm results in a shove.
        /// </summary>
        public float StaminaDamage { get; init; }
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }
}
