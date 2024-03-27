using System.Linq;
using Content.Server.Light.Components;
using Content.Shared._EstacaoPirata.Attachables;
using Content.Shared._EstacaoPirata.Attachments;
using Content.Shared.Actions;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Hands;
using Content.Shared.Interaction;
using Content.Shared.Inventory.Events;
using Content.Shared.Light;
using Content.Shared.Light.Components;
using Content.Shared.Popups;
using Robust.Server.GameObjects;
using Robust.Shared.Containers;

namespace Content.Server._EstacaoPirata.Attachables;

/// <summary>
/// This handles...
/// </summary>
public sealed class AttachableSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _sharedActionsSystem = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearanceSystem = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<AttachableSlotComponent, EntInsertedIntoContainerMessage>(OnEntityInserted);
        SubscribeLocalEvent<AttachableSlotComponent, GetItemActionsEvent>(OnItemEquipped);
        SubscribeLocalEvent<AttachableSlotComponent, GotUnequippedEvent>(OnGotUnequipped);
        SubscribeLocalEvent<AttachableSlotComponent, GotUnequippedHandEvent>(OnGotUnequippedHands);
        SubscribeLocalEvent<AttachableSlotComponent, EntRemovedFromContainerMessage>(OnEntityRemoved);
        SubscribeLocalEvent<AttachmentComponent, InstantActionEvent>(OnAction);
    }

    private void OnGotUnequippedHands(EntityUid uid, AttachableSlotComponent component, ref GotUnequippedHandEvent args)
    {
        TransferActionsBack(component, args.Unequipped);
    }

    private void OnGotUnequipped(EntityUid uid, AttachableSlotComponent component, ref GotUnequippedEvent args)
    {
        TransferActionsBack(component, args.Equipment);
    }

    private void TransferActionsBack(AttachableSlotComponent component, EntityUid unequipped)
    {
        if (!TryComp<AttachableSlotComponent>(unequipped, out var attachableSlotComponent))
            return;
        if (attachableSlotComponent.AttachedItem == null)
            return;

        foreach (var action in component.AttachedItemActions)
        {
            _sharedActionsSystem.AddActionDirect(performer: attachableSlotComponent.AttachedItem.Value, actionId: action);
        }

        component.PlayerWithItemEquipped = null;
        component.AttachedItemActions.Clear();
    }

    private void OnItemEquipped(EntityUid uid, AttachableSlotComponent component, ref GetItemActionsEvent args)
    {
        Log.Debug($"OnItemEquipped {args.SlotFlags}");
        if (!TryComp<AttachableSlotComponent>(args.Provider, out var attachableSlotComponent))
            return;

        // ver depois em que linha colocar isto
        component.PlayerWithItemEquipped = args.User;

        if (attachableSlotComponent.AttachedItem == null)
            return;

        if (!TryComp<ActionsComponent>(attachableSlotComponent.AttachedItem.Value, out var actionsComponent))
            return;

        foreach (var action in actionsComponent.Actions)
        {
            if (!component.AttachedItemActions.Contains(action))
            {
                component.AttachedItemActions.Add(action);
                _sharedActionsSystem.AddActionDirect(performer: args.User, actionId: action);
            }
        }
    }

    private void OnEntityInserted(EntityUid uid, AttachableSlotComponent slotComponent, EntInsertedIntoContainerMessage args)
    {
        if (args.Container.ID != slotComponent.AttachableSlotId)
            return;

        slotComponent.AttachedItem = args.Entity;

        _appearanceSystem.SetData(uid, AttachableVisuals.VisualState, AttachableVisualLayers.Attachment);

        // Checar se e um attachment que ilumina
        args.Container.OccludesLight = false;

        if (!TryComp<ActionsComponent>(slotComponent.AttachedItem.Value, out var actionsComponent))
            return;

        if (slotComponent.PlayerWithItemEquipped == null)
            return;

        foreach (var action in actionsComponent.Actions)
        {
            if (!slotComponent.AttachedItemActions.Contains(action))
            {
                slotComponent.AttachedItemActions.Add(action);
                _sharedActionsSystem.AddActionDirect(performer: slotComponent.PlayerWithItemEquipped.Value, actionId: action);
            }
        }
    }

    private void OnEntityRemoved(EntityUid uid, AttachableSlotComponent slotComponent, EntRemovedFromContainerMessage args)
    {
        if (args.Container.ID != slotComponent.AttachableSlotId)
            return;

        if (slotComponent.AttachedItem == null)
            return;

        _appearanceSystem.SetData(uid, AttachableVisuals.VisualState, AttachableVisualLayers.Base);

        if (slotComponent.PlayerWithItemEquipped == null)
            return;

        if (!TryComp<AttachableSlotComponent>(uid, out var attachableSlotComponent))
            return;

        foreach (var action in attachableSlotComponent.AttachedItemActions)
        {
            _sharedActionsSystem.AddActionDirect(performer: slotComponent.AttachedItem.Value, actionId: action);
        }

        slotComponent.AttachedItemActions.Clear();
        slotComponent.AttachedItem = null;
    }

    private void OnAction(EntityUid uid, AttachmentComponent component, InstantActionEvent args)
    {
        if (!HasComp<AttachmentComponent>(args.Performer))
            return;

        _appearanceSystem.SetData(component.Owner, AttachableVisuals.VisualState, AttachableVisualLayers.AttachmentActivated);
    }
}
