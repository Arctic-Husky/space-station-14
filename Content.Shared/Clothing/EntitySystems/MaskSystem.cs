using Content.Shared.Actions;
using Content.Shared.Clothing.Components;
using Content.Shared.Foldable;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Content.Shared.Item;
using Content.Shared.Popups;
using Robust.Shared.Timing;

namespace Content.Shared.Clothing.EntitySystems;

public sealed class MaskSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actionSystem = default!;
    [Dependency] private readonly InventorySystem _inventorySystem = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MaskComponent, ToggleMaskEvent>(OnToggleMask);
        SubscribeLocalEvent<MaskComponent, GetItemActionsEvent>(OnGetActions);
        SubscribeLocalEvent<MaskComponent, GotUnequippedEvent>(OnGotUnequipped);
        SubscribeLocalEvent<MaskComponent, FoldedEvent>(OnFolded);
    }

    private void OnGetActions(EntityUid uid, MaskComponent component, GetItemActionsEvent args)
    {
        if (_inventorySystem.InSlotWithFlags(uid, SlotFlags.MASK))
            args.AddAction(ref component.ToggleActionEntity, component.ToggleAction);
    }

    private void OnToggleMask(Entity<MaskComponent> ent, ref ToggleMaskEvent args)
    {
        var (uid, mask) = ent;
        if (mask.ToggleActionEntity == null || !_timing.IsFirstTimePredicted)
            return;

        if (!_inventorySystem.TryGetSlotEntity(args.Performer, "mask", out var existing) || !uid.Equals(existing))
            return;

        mask.IsToggled ^= true;
<<<<<<< HEAD

        var dir = mask.IsToggled ? "down" : "up";
        var msg = $"action-mask-pull-{dir}-popup-message";
        _popupSystem.PopupClient(Loc.GetString(msg, ("mask", uid)), args.Performer, args.Performer);
=======
        _actionSystem.SetToggled(mask.ToggleActionEntity, mask.IsToggled);

        if (mask.IsToggled)
            _popupSystem.PopupEntity(Loc.GetString("action-mask-pull-down-popup-message", ("mask", uid)), args.Performer, args.Performer);
        else
            _popupSystem.PopupEntity(Loc.GetString("action-mask-pull-up-popup-message", ("mask", uid)), args.Performer, args.Performer);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

        ToggleMaskComponents(uid, mask, args.Performer, mask.EquippedPrefix);
    }

    // set to untoggled when unequipped, so it isn't left in a 'pulled down' state
    private void OnGotUnequipped(EntityUid uid, MaskComponent mask, GotUnequippedEvent args)
    {
<<<<<<< HEAD
        if (!mask.IsToggled)
            return;

        mask.IsToggled = false;
        ToggleMaskComponents(uid, mask, args.Equipee, mask.EquippedPrefix, true);
    }

    /// <summary>
    /// Called after setting IsToggled, raises events and dirties.
    /// <summary>
    private void ToggleMaskComponents(EntityUid uid, MaskComponent mask, EntityUid wearer, string? equippedPrefix = null, bool isEquip = false)
    {
        Dirty(uid, mask);
        if (mask.ToggleActionEntity is {} action)
            _actionSystem.SetToggled(action, mask.IsToggled);

=======
        if (mask.ToggleActionEntity == null)
            return;

        mask.IsToggled = false;
        Dirty(uid, mask);
        _actionSystem.SetToggled(mask.ToggleActionEntity, mask.IsToggled);

        ToggleMaskComponents(uid, mask, args.Equipee, mask.EquippedPrefix, true);
    }

    private void ToggleMaskComponents(EntityUid uid, MaskComponent mask, EntityUid wearer, string? equippedPrefix = null, bool isEquip = false)
    {
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        var maskEv = new ItemMaskToggledEvent(wearer, equippedPrefix, mask.IsToggled, isEquip);
        RaiseLocalEvent(uid, ref maskEv);

        var wearerEv = new WearerMaskToggledEvent(mask.IsToggled);
        RaiseLocalEvent(wearer, ref wearerEv);
    }

    private void OnFolded(Entity<MaskComponent> ent, ref FoldedEvent args)
    {
        ent.Comp.IsToggled = args.IsFolded;

        ToggleMaskComponents(ent.Owner, ent.Comp, ent.Owner);
    }
}
