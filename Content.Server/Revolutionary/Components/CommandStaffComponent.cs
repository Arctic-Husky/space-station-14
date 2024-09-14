using Content.Server.GameTicking.Rules;

namespace Content.Server.Revolutionary.Components;

/// <summary>
<<<<<<< HEAD
/// Given to heads at round start for Revs. Used for tracking if heads died or not.
=======
///     Component for tracking if someone is a Head of Staff.
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
/// </summary>
[RegisterComponent, Access(typeof(RevolutionaryRuleSystem))]
public sealed partial class CommandStaffComponent : Component
{
<<<<<<< HEAD

=======
    public float PsionicBonusModifier = 1;
    public float PsionicBonusOffset = 0.25f;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
}
