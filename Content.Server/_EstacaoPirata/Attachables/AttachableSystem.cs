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
    // [Dependency] private readonly SharedPopupSystem _popup = default!;
    // [Dependency] private readonly SharedContainerSystem _containerSystem = default!;
    // [Dependency] private readonly ItemSlotsSystem _itemSlotsSystem = default!;
    [Dependency] private readonly SharedActionsSystem _sharedActionsSystem = default!;
    [Dependency] private readonly SharedContainerSystem _sharedContainerSystem = default!;
    [Dependency] private readonly ActionContainerSystem _actionContainerSystem = default!;
    [Dependency] private readonly SharedPointLightSystem _lights = default!;
    [Dependency] private readonly SharedHandheldLightSystem _handheldLightSystem = default!;




    public override void Initialize()
    {
        SubscribeLocalEvent<AttachableSlotComponent, EntInsertedIntoContainerMessage>(OnEntityInserted);
        SubscribeLocalEvent<AttachableSlotComponent, GetItemActionsEvent>(OnItemEquipped);
        SubscribeLocalEvent<AttachableSlotComponent, GotUnequippedEvent>(OnGotUnequipped);
        SubscribeLocalEvent<AttachableSlotComponent, GotUnequippedHandEvent>(OnGotUnequippedHands);
        //SubscribeLocalEvent<ActionsComponent, DidEquipEvent>(OnDidEquip);
        //SubscribeLocalEvent<AttachableSlotComponent, ItemSlotInsertAttemptEvent>(OnInsertAttempt);
        //SubscribeLocalEvent<AttachableSlotComponent, InteractUsingEvent>(OnInteractUsing);
        //SubscribeLocalEvent<AttachableSlotComponent, ContainerIsInsertingAttemptEvent>(OnContainerInsertAttempt);
    }

    private void OnGotUnequippedHands(EntityUid uid, AttachableSlotComponent component, ref GotUnequippedHandEvent args)
    {
        Log.Debug("OnGotUnequippedHand");
        if (TryComp<AttachableSlotComponent>(args.Unequipped, out var attachableSlotComponent))
        {
            if (attachableSlotComponent.AttachedItem != null)
            {
                var item = attachableSlotComponent.AttachedItem.Value;
                _sharedActionsSystem.RemoveAction();
                _sharedActionsSystem.RemoveProvidedActions(performer:item , container:item);
            }
        }
    }

    private void OnGotUnequipped(EntityUid uid, AttachableSlotComponent component, ref GotUnequippedEvent args)
    {
        Log.Debug("OnGotUnequipped");
        if (TryComp<AttachableSlotComponent>(args.Equipment, out var attachableSlotComponent))
        {
            if (attachableSlotComponent.AttachedItem != null)
            {
                var item = attachableSlotComponent.AttachedItem.Value;
                _sharedActionsSystem.RemoveProvidedActions(performer:item , container:item);
            }
        }
    }

    private void OnItemEquipped(EntityUid uid, AttachableSlotComponent component, ref GetItemActionsEvent args)
    {
        // Aaaaah sei la

        if (TryComp<AttachableSlotComponent>(args.Provider, out var attachableSlotComponent))
        {
            if (attachableSlotComponent.AttachedItem != null)
            {
                _sharedActionsSystem.GrantContainedActions(args.User, attachableSlotComponent.AttachedItem.Value);
            }
        }
    }

    private void OnEntityInserted(EntityUid uid, AttachableSlotComponent slotComponent, EntInsertedIntoContainerMessage args)
    {
        if (args.Container.ID != slotComponent.AttachableSlotId)
            return;

        slotComponent.AttachedItem = args.Entity;

        args.Container.OccludesLight = false;
    }
}
