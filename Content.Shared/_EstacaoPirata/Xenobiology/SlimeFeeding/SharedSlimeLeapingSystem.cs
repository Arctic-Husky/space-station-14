using System.Numerics;
using Content.Shared.Buckle;
using Content.Shared.Interaction.Components;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Movement.Pulling.Systems;
using Content.Shared.StatusEffect;
using Content.Shared.Stunnable;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Timing;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeFeeding;

/// <summary>
/// This handles...
/// </summary>
public sealed class SharedSlimeLeapingSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly PullingSystem _pulling = default!;
    [Dependency] private readonly SharedStunSystem _stunSystem = default!;
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;
    [Dependency] private readonly SharedBroadphaseSystem _broadphase = default!;
    [Dependency] private readonly StatusEffectsSystem _statusEffect = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;

    private EntityQuery<PhysicsComponent> _physicsQuery;

    public override void Initialize()
    {
        _physicsQuery = GetEntityQuery<PhysicsComponent>();

        SubscribeLocalEvent<SlimeLeapingComponent, StartCollideEvent>(OnLeapingDoHit);
        SubscribeLocalEvent<SlimeLeapingComponent, ComponentRemove>(OnSlimeLeapingRemove);
        SubscribeLocalEvent<SlimeLeapingComponent, PhysicsSleepEvent>(OnSlimeLeapingPhysicsSleep);
        SubscribeLocalEvent<SlimeFeedingComponent, UnlatchOnEvent>(OnUnlatch);

        SubscribeLocalEvent<SlimeFeedingIncapacitatedComponent, ComponentShutdown>(OnIncapacitatedShutdown);

    }

    public override void Update(float frameTime)
    {
        var time = _timing.CurTime;
        var leaping = EntityQueryEnumerator<SlimeLeapingComponent>();
        while (leaping.MoveNext(out var uid, out var comp))
        {
            if (time < comp.LeapEndTime)
                continue;

            StopLeap(uid);

            RemCompDeferred<SlimeLeapingComponent>(uid);
        }
    }

    public bool LeapToTarget(EntityUid user, EntityUid target)
    {
        Log.Debug($"Leap To Target {user} -> {target}");
        if (!_physicsQuery.TryGetComponent(user, out var physics))
            return false;

        if (EnsureComp<SlimeLeapingComponent>(user, out var leaping))
            return false;

        if (!TryComp<SlimeFeedingComponent>(user, out var slimeFeedingComponent))
            return false;

        if (!TryComp<SlimeFoodComponent>(target, out var slimeFoodComponent))
            return false;

        if (TryComp(user, out PullerComponent? puller) && TryComp(puller.Pulling, out PullableComponent? pullable))
            _pulling.TryStopPull(puller.Pulling.Value, pullable, user);

        var origin = _transform.GetMapCoordinates(user);
        var targetCoords = _transform.GetMapCoordinates(target);//GetCoordinates(args.Coordinates).ToMap(EntityManager, _transform);
        var direction = targetCoords.Position - origin.Position;

        if (direction == Vector2.Zero)
            return false;

        var length = direction.Length();
        var distance = Math.Clamp(length, 0.1f, slimeFeedingComponent.LeapDistance);
        direction *= distance / length;
        var impulse = direction.Normalized() * slimeFeedingComponent.LeapStrength * physics.Mass;

        leaping.Origin = _transform.GetMoverCoordinates(user);
        leaping.LeapEndTime = _timing.CurTime + TimeSpan.FromSeconds(direction.Length() / slimeFeedingComponent.LeapStrength);

        _physics.ApplyLinearImpulse(user, impulse, body: physics);
        _physics.SetBodyStatus(user, physics, BodyStatus.InAir);

        return true;
    }

    private void OnLeapingDoHit(Entity<SlimeLeapingComponent> ent, ref StartCollideEvent args)
    {
        Log.Debug($"Leaping Do Hit");
        RemComp<SlimeLeapingComponent>(ent.Owner);

        var target = args.OtherEntity;

        if (_physicsQuery.TryGetComponent(ent.Owner, out var physics))
        {
            _physics.SetBodyStatus(ent.Owner, physics, BodyStatus.OnGround);

            if (physics.Awake)
                _broadphase.RegenerateContacts(ent.Owner, physics);
        }

        LatchOn(ent.Owner, target);
    }

    public bool LatchOn(EntityUid user, EntityUid target)
    {
        Log.Debug($"Latching de {user} em {target}");
        if (user == target)
            return false;

        if (!TryComp<SlimeFeedingComponent>(user, out var slimeFeedingComponent))
            return false;

        if (!TryComp<SlimeFoodComponent>(target, out var slimeFoodComponent))
            return false;

        if (HasComp<SlimeBadFoodComponent>(target))
            return false;

        if (EnsureComp<SlimeFeedingIncapacitatedComponent>(target, out var victim))
            return false;

        victim.Attacker = user;

        AddComp<BlockMovementComponent>(target);
        _stunSystem.TryKnockdown(target, slimeFeedingComponent.LatchOnTime, true);

        slimeFeedingComponent.Victim = target;

        var latchOnEvent = new LatchOnEvent(user, target);
        RaiseLocalEvent(user, latchOnEvent);

        return true;
    }

    private void StopLeap(EntityUid user)
    {
        Log.Debug($"Parando Leap em {user}");
        if (_physicsQuery.TryGetComponent(user, out var physics))
        {
            _physics.SetLinearVelocity(user, Vector2.Zero, body: physics);
            _physics.SetBodyStatus(user, physics, BodyStatus.OnGround);
        }

        var mapCoordinates = _transform.GetMapCoordinates(user);

        var entitiesIntersecting = _lookup.GetEntitiesIntersecting(mapCoordinates, LookupFlags.Uncontained);

        // O(n)
        foreach (var ent in entitiesIntersecting)
        {
            if (!HasComp<SlimeFoodComponent>(ent) || ent == user)
                continue;

            if (HasComp<SlimeBadFoodComponent>(ent))
                continue;

            LatchOn(user, ent);
            return;
        }
    }

    // Maybe change this to not be related to feeding
    private void OnUnlatch(EntityUid uid, SlimeFeedingComponent component, ref UnlatchOnEvent args)
    {
        RemComp<BlockMovementComponent>(args.Target);
        RemComp<SlimeFeedingIncapacitatedComponent>(args.Target);
        _statusEffect.TryRemoveStatusEffect(args.Target, "KnockedDown");
        Log.Debug($"Unlatching {component.Victim}");
        component.Victim = null;
        component.VictimResisted = false;
    }

    private void OnSlimeLeapingRemove(Entity<SlimeLeapingComponent> ent, ref ComponentRemove args)
    {
        Log.Debug($"SlimeLeaping ComponentRemove");
        StopLeap(ent.Owner);
    }

    private void OnSlimeLeapingPhysicsSleep(Entity<SlimeLeapingComponent> ent, ref PhysicsSleepEvent args)
    {
        Log.Debug($"Physics Sleep");
        StopLeap(ent.Owner);
    }

    private void OnIncapacitatedShutdown(EntityUid uid, SlimeFeedingIncapacitatedComponent component, ComponentShutdown args)
    {
        Log.Debug($"SlimeLeaping ComponentShutdown");
        StopLeap(uid);
    }
}
