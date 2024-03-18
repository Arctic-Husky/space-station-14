using Content.Shared._EstacaoPirata.Attachments;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Inventory.Events;
using Content.Shared.Popups;
using Robust.Shared.Containers;

namespace Content.Shared._EstacaoPirata.Attachables;

/// <summary>
/// This handles...
/// </summary>
public sealed class AttachableSystem : EntitySystem
{

    public override void Initialize()
    {
        SubscribeLocalEvent<AttachableSlotComponent, ContainerIsInsertingAttemptEvent>(OnContainerInsertAttempt);
        //SubscribeLocalEvent<AttachableSlotComponent, GotUnequippedEvent>(OnGotUnequipped);
    }

    // private void OnGotUnequipped(EntityUid uid, AttachableSlotComponent component, ref GotUnequippedEvent args)
    // {
    //     Log.Debug("OnGotUnequipped");
    // }

    private void OnContainerInsertAttempt(EntityUid uid, AttachableSlotComponent component, ref ContainerIsInsertingAttemptEvent args)
    {
        if (!component.Initialized)
        {
            args.Cancel();
            return;
        }

        if (args.Container.ID != component.AttachableSlotId)
        {
            args.Cancel();
            return;
        }

        if (!TryComp<AttachmentComponent>(args.EntityUid, out var attachmentComponent))
        {
            args.Cancel();
            return;
        }

        if (attachmentComponent.AttachmentSlotId != component.AttachableSlotId)
        {
            args.Cancel();
            return;
        }
    }
}
