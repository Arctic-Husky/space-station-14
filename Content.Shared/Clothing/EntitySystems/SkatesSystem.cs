using Content.Shared.Inventory.Events;
using Content.Shared.Movement.Systems;
using Content.Shared.Damage.Systems;
using Content.Shared.Movement.Components;

namespace Content.Shared.Clothing;

/// <summary>
/// Changes the friction and acceleration of the wearer and also the damage on impact variables of thew wearer when hitting a static object.
/// </summary>
public sealed class SkatesSystem : EntitySystem
{
    [Dependency] private readonly MovementSpeedModifierSystem _move = default!;
    [Dependency] private readonly DamageOnHighSpeedImpactSystem _impact = default!;

    public override void Initialize()
    {
        base.Initialize();

<<<<<<< HEAD
        SubscribeLocalEvent<SkatesComponent, ClothingGotEquippedEvent>(OnGotEquipped);
        SubscribeLocalEvent<SkatesComponent, ClothingGotUnequippedEvent>(OnGotUnequipped);
=======
        SubscribeLocalEvent<SkatesComponent, GotEquippedEvent>(OnGotEquipped);
        SubscribeLocalEvent<SkatesComponent, GotUnequippedEvent>(OnGotUnequipped);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }

    /// <summary>
    /// When item is unequipped from the shoe slot, friction, aceleration and collide on impact return to default settings.
    /// </summary>
<<<<<<< HEAD
    public void OnGotUnequipped(EntityUid uid, SkatesComponent component, ClothingGotUnequippedEvent args)
    {
        if (!TryComp(args.Wearer, out MovementSpeedModifierComponent? speedModifier))
            return;

        _move.ChangeFriction(args.Wearer, MovementSpeedModifierComponent.DefaultFriction, MovementSpeedModifierComponent.DefaultFrictionNoInput, MovementSpeedModifierComponent.DefaultAcceleration, speedModifier);
        _impact.ChangeCollide(args.Wearer, component.DefaultMinimumSpeed, component.DefaultStunSeconds, component.DefaultDamageCooldown, component.DefaultSpeedDamage);
=======
    public void OnGotUnequipped(EntityUid uid, SkatesComponent component, GotUnequippedEvent args)
    {
        if (!TryComp(args.Equipee, out MovementSpeedModifierComponent? speedModifier))
            return;

        if (args.Slot == "shoes")
        {
            _move.ChangeFriction(args.Equipee, MovementSpeedModifierComponent.DefaultFriction, MovementSpeedModifierComponent.DefaultFrictionNoInput, MovementSpeedModifierComponent.DefaultAcceleration, speedModifier);
            _impact.ChangeCollide(args.Equipee, component.DefaultMinimumSpeed, component.DefaultStunSeconds, component.DefaultDamageCooldown, component.DefaultSpeedDamage);
        }
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }

    /// <summary>
    /// When item is equipped into the shoe slot, friction, acceleration and collide on impact are adjusted.
    /// </summary>
<<<<<<< HEAD
    private void OnGotEquipped(EntityUid uid, SkatesComponent component, ClothingGotEquippedEvent args)
    {
        _move.ChangeFriction(args.Wearer, component.Friction, component.FrictionNoInput, component.Acceleration);
        _impact.ChangeCollide(args.Wearer, component.MinimumSpeed, component.StunSeconds, component.DamageCooldown, component.SpeedDamage);
=======
    private void OnGotEquipped(EntityUid uid, SkatesComponent component, GotEquippedEvent args)
    {
        if (args.Slot == "shoes")
        { 
            _move.ChangeFriction(args.Equipee, component.Friction, component.FrictionNoInput, component.Acceleration);
            _impact.ChangeCollide(args.Equipee, component.MinimumSpeed, component.StunSeconds, component.DamageCooldown, component.SpeedDamage);
        }
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }
}
