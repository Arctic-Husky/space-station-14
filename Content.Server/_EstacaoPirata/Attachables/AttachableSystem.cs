using Content.Shared._EstacaoPirata.Attachables;
using Content.Shared._EstacaoPirata.Attachments;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Robust.Shared.Containers;

namespace Content.Server._EstacaoPirata.Attachables;

/// <summary>
/// This handles...
/// </summary>
public sealed class AttachableSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedContainerSystem _containerSystem = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlotsSystem = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<AttachableSlotComponent, EntInsertedIntoContainerMessage>(OnEntityInserted);
        //SubscribeLocalEvent<AttachableSlotComponent, ItemSlotInsertAttemptEvent>(OnInsertAttempt);
        //SubscribeLocalEvent<AttachableSlotComponent, InteractUsingEvent>(OnInteractUsing);
        //SubscribeLocalEvent<AttachableSlotComponent, ContainerIsInsertingAttemptEvent>(OnContainerInsertAttempt);
    }

    private void OnEntityInserted(EntityUid uid, AttachableSlotComponent slotComponent, EntInsertedIntoContainerMessage args)
    {
        if (args.Container.ID != slotComponent.AttachableSlotId)
            return;

        // Give the player an action to activate the inserted item's action
    }
}
