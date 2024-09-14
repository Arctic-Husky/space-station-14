<<<<<<< HEAD
using Content.Server.NPC.Components;
using Content.Shared.CombatMode;
using Content.Shared.Damage;
using Content.Shared.Mobs.Components;
using Content.Shared.NPC.Components;
using Content.Shared.NPC.Systems;
=======
﻿using Content.Server.NPC.Components;
using Content.Shared.CombatMode;
using Content.Shared.Damage;
using Content.Shared.Mobs.Components;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
using Robust.Shared.Collections;
using Robust.Shared.Timing;

namespace Content.Server.NPC.Systems;

/// <summary>
///     Handles NPC which become aggressive after being attacked.
/// </summary>
public sealed class NPCRetaliationSystem : EntitySystem
{
    [Dependency] private readonly NpcFactionSystem _npcFaction = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    /// <inheritdoc />
    public override void Initialize()
    {
        SubscribeLocalEvent<NPCRetaliationComponent, DamageChangedEvent>(OnDamageChanged);
        SubscribeLocalEvent<NPCRetaliationComponent, DisarmedEvent>(OnDisarmed);
    }

<<<<<<< HEAD
    private void OnDamageChanged(Entity<NPCRetaliationComponent> ent, ref DamageChangedEvent args)
=======
    private void OnDamageChanged(EntityUid uid, NPCRetaliationComponent component, DamageChangedEvent args)
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    {
        if (!args.DamageIncreased)
            return;

<<<<<<< HEAD
        if (args.Origin is not {} origin)
            return;

        TryRetaliate(ent, origin);
    }

    private void OnDisarmed(Entity<NPCRetaliationComponent> ent, ref DisarmedEvent args)
    {
        TryRetaliate(ent, args.Source);
    }

    public bool TryRetaliate(Entity<NPCRetaliationComponent> ent, EntityUid target)
    {
=======
        if (args.Origin is not { } origin)
            return;

        TryRetaliate(uid, origin, component);
    }

    private void OnDisarmed(EntityUid uid, NPCRetaliationComponent component, DisarmedEvent args)
    {
        TryRetaliate(uid, args.Source, component);
    }

    public bool TryRetaliate(EntityUid uid, EntityUid target, NPCRetaliationComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return false;

>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        // don't retaliate against inanimate objects.
        if (!HasComp<MobStateComponent>(target))
            return false;

<<<<<<< HEAD
        // don't retaliate against the same faction
        if (_npcFaction.IsEntityFriendly(ent.Owner, target))
            return false;

        _npcFaction.AggroEntity(ent.Owner, target);
        if (ent.Comp.AttackMemoryLength is {} memoryLength)
            ent.Comp.AttackMemories[target] = _timing.CurTime + memoryLength;
=======
        if (_npcFaction.IsEntityFriendly(uid, target))
            return false;

        _npcFaction.AggroEntity(uid, target);
        if (component.AttackMemoryLength is { } memoryLength)
            component.AttackMemories[target] = _timing.CurTime + memoryLength;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

        return true;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<NPCRetaliationComponent, FactionExceptionComponent>();
        while (query.MoveNext(out var uid, out var retaliationComponent, out var factionException))
        {
<<<<<<< HEAD
            // TODO: can probably reuse this allocation and clear it
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            foreach (var entity in new ValueList<EntityUid>(retaliationComponent.AttackMemories.Keys))
            {
                if (!TerminatingOrDeleted(entity) && _timing.CurTime < retaliationComponent.AttackMemories[entity])
                    continue;

<<<<<<< HEAD
                _npcFaction.DeAggroEntity((uid, factionException), entity);
                // TODO: should probably remove the AttackMemory, thats the whole point of the ValueList right??
=======
                _npcFaction.DeAggroEntity(uid, entity, factionException);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            }
        }
    }
}
