using Content.Shared.Damage;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Weapons.Melee;

/// <summary>
/// When given to a mob lets them do unarmed attacks, or when given to an item lets someone wield it to do attacks.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class MeleeWeaponComponent : Component
{
    // TODO: This is becoming bloated as shit.
    // This should just be its own component for alt attacks.
    /// <summary>
    /// Does this entity do a disarm on alt attack.
    /// </summary>
<<<<<<< HEAD
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
=======
    [DataField, AutoNetworkedField]
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    public bool AltDisarm = true;

    /// <summary>
    /// Should the melee weapon's damage stats be examinable.
    /// </summary>
<<<<<<< HEAD
    [ViewVariables(VVAccess.ReadWrite)]
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    [DataField]
    public bool Hidden;

    /// <summary>
    /// Next time this component is allowed to light attack. Heavy attacks are wound up and never have a cooldown.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField]
<<<<<<< HEAD
    [ViewVariables(VVAccess.ReadWrite)]
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    [AutoPausedField]
    public TimeSpan NextAttack;

    /// <summary>
    /// Starts attack cooldown when equipped if true.
    /// </summary>
<<<<<<< HEAD
    [ViewVariables(VVAccess.ReadWrite), DataField]
=======
    [DataField]
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    public bool ResetOnHandSelected = true;

    /*
     * Melee combat works based around 2 types of attacks:
     * 1. Click attacks with left-click. This attacks whatever is under your mnouse
     * 2. Wide attacks with right-click + left-click. This attacks whatever is in the direction of your mouse.
     */

    /// <summary>
    /// How many times we can attack per second.
    /// </summary>
<<<<<<< HEAD
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
=======
    [DataField, AutoNetworkedField]
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    public float AttackRate = 1f;

    /// <summary>
    ///     When power attacking, the swing speed (in attacks per second) is multiplied by this amount
    /// </summary>
    [DataField, AutoNetworkedField]
    public float HeavyRateModifier = 0.8f;
    /// <summary>
    /// Are we currently holding down the mouse for an attack.
    /// Used so we can't just hold the mouse button and attack constantly.
    /// </summary>
<<<<<<< HEAD
    [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
=======
    [AutoNetworkedField]
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    public bool Attacking = false;

    /// <summary>
    /// If true, attacks will be repeated automatically without requiring the mouse button to be lifted.
    /// </summary>
<<<<<<< HEAD
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
=======
    [DataField, AutoNetworkedField]
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    public bool AutoAttack;

    /// <summary>
    /// Base damage for this weapon. Can be modified via heavy damage or other means.
    /// </summary>
    [DataField(required: true)]
<<<<<<< HEAD
    [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public DamageSpecifier Damage = default!;

    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 BluntStaminaDamageFactor = FixedPoint2.New(0.5f);
=======
    [AutoNetworkedField]
    public DamageSpecifier Damage = default!;

    [DataField, AutoNetworkedField]
    public FixedPoint2 BluntStaminaDamageFactor = FixedPoint2.New(1f);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    /// <summary>
    /// Multiplies damage by this amount for single-target attacks.
    /// </summary>
<<<<<<< HEAD
    [ViewVariables(VVAccess.ReadWrite), DataField]
=======
    [DataField, AutoNetworkedField]
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    public FixedPoint2 ClickDamageModifier = FixedPoint2.New(1);

    // TODO: Temporarily 1.5 until interactionoutline is adjusted to use melee, then probably drop to 1.2
    /// <summary>
    /// Nearest edge range to hit an entity.
    /// </summary>
<<<<<<< HEAD
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
=======
    [DataField, AutoNetworkedField]
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    public float Range = 1.5f;

    /// <summary>
    ///     Attack range for heavy swings
    /// </summary>
    [DataField, AutoNetworkedField]
    public float HeavyRangeModifier = 1f;

    /// <summary>
    ///     Weapon damage is multiplied by this amount for heavy swings
    /// </summary>
    [DataField, AutoNetworkedField]
    public float HeavyDamageBaseModifier = 1.2f;

    /// <summary>
    /// Total width of the angle for wide attacks.
    /// </summary>
<<<<<<< HEAD
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public Angle Angle = Angle.FromDegrees(60);

    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public EntProtoId Animation = "WeaponArcPunch";

    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public EntProtoId WideAnimation = "WeaponArcSlash";

    /// <summary>
    /// Rotation of the animation.
    /// 0 degrees means the top faces the attacker.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public Angle WideAnimationRotation = Angle.Zero;

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public bool SwingLeft;

=======
    [DataField, AutoNetworkedField]
    public Angle Angle = Angle.FromDegrees(45);

    [DataField, AutoNetworkedField]
    public EntProtoId Animation = "WeaponArcPunch";

    [DataField, AutoNetworkedField]
    public EntProtoId WideAnimation = "WeaponArcSlash";

    /// <summary>
    /// Rotation of the animation.
    /// 0 degrees means the top faces the attacker.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Angle WideAnimationRotation = Angle.Zero;

    [DataField]
    public bool SwingLeft;

    [DataField, AutoNetworkedField]
    public float HeavyStaminaCost = 10f;

    [DataField, AutoNetworkedField]
    public int MaxTargets = 1;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    // Sounds

    /// <summary>
    /// This gets played whenever a melee attack is done. This is predicted by the client.
    /// </summary>
<<<<<<< HEAD
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("soundSwing"), AutoNetworkedField]
    public SoundSpecifier SwingSound { get; set; } = new SoundPathSpecifier("/Audio/Weapons/punchmiss.ogg")
=======
    [DataField, AutoNetworkedField]
    public SoundSpecifier SoundSwing { get; set; } = new SoundPathSpecifier("/Audio/Weapons/punchmiss.ogg")
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    {
        Params = AudioParams.Default.WithVolume(-3f).WithVariation(0.025f),
    };

    // We do not predict the below sounds in case the client thinks but the server disagrees. If this were the case
    // then a player may doubt if the target actually took damage or not.
    // If overwatch and apex do this then we probably should too.

<<<<<<< HEAD
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("soundHit"), AutoNetworkedField]
    public SoundSpecifier? HitSound;
=======
    [DataField, AutoNetworkedField]
    public SoundSpecifier? SoundHit;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    /// <summary>
    /// Plays if no damage is done to the target entity.
    /// </summary>
<<<<<<< HEAD
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("soundNoDamage"), AutoNetworkedField]
    public SoundSpecifier NoDamageSound { get; set; } = new SoundCollectionSpecifier("WeakHit");

    /// <summary>
    /// If true, the weapon must be equipped for it to be used.
    /// E.g boxing gloves must be equipped to your gloves,
    /// not just held in your hand to be used.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool MustBeEquippedToUse = false;
=======
    [DataField, AutoNetworkedField]
    public SoundSpecifier SoundNoDamage { get; set; } = new SoundCollectionSpecifier("WeakHit");
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
}

/// <summary>
/// Event raised on entity in GetWeapon function to allow systems to manually
/// specify what the weapon should be.
/// </summary>
public sealed class GetMeleeWeaponEvent : HandledEntityEventArgs
{
    public EntityUid? Weapon;
}
