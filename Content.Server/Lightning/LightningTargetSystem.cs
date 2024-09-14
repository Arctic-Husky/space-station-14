using Content.Server.Explosion.EntitySystems;
using Content.Server.Lightning;
using Content.Server.Lightning.Components;
using Content.Shared.Damage;
<<<<<<< HEAD
using Robust.Server.GameObjects;
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

namespace Content.Server.Tesla.EntitySystems;

/// <summary>
/// The component allows lightning to strike this target. And determining the behavior of the target when struck by lightning.
/// </summary>
public sealed class LightningTargetSystem : EntitySystem
{
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly ExplosionSystem _explosionSystem = default!;
<<<<<<< HEAD
    [Dependency] private readonly TransformSystem _transform = default!;
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LightningTargetComponent, HitByLightningEvent>(OnHitByLightning);
    }

    private void OnHitByLightning(Entity<LightningTargetComponent> uid, ref HitByLightningEvent args)
    {
        DamageSpecifier damage = new();
        damage.DamageDict.Add("Structural", uid.Comp.DamageFromLightning);
        _damageable.TryChangeDamage(uid, damage, true);

        if (uid.Comp.LightningExplode)
        {
            _explosionSystem.QueueExplosion(
<<<<<<< HEAD
                _transform.GetMapCoordinates(uid),
=======
                Transform(uid).MapPosition,
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
                uid.Comp.ExplosionPrototype,
                uid.Comp.TotalIntensity, uid.Comp.Dropoff,
                uid.Comp.MaxTileIntensity,
                canCreateVacuum: false);
        }
    }
}
