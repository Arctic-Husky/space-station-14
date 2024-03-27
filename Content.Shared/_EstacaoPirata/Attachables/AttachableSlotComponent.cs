using Content.Shared.Containers.ItemSlots;
using Robust.Shared.Serialization;

namespace Content.Shared._EstacaoPirata.Attachables;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class AttachableSlotComponent : Component
{
    /// <summary>
    /// The actual item-slot that contains the cell. Allows all the interaction logic to be handled by <see cref="ItemSlotsSystem"/>.
    /// </summary>
    [DataField("attachableSlotId", required: true)]
    public string AttachableSlotId = string.Empty;

    [ViewVariables]
    public EntityUid? AttachedItem;

    [ViewVariables]
    public HashSet<EntityUid> AttachedItemActions = new();

    [ViewVariables]
    public EntityUid? PlayerWithItemEquipped;
}

[Serializable, NetSerializable]
public enum AttachableVisuals
{
    VisualState
}

[Serializable, NetSerializable]
public enum AttachableVisualLayers : byte
{
    Base,
    Attachment,
    AttachmentActivated
}
