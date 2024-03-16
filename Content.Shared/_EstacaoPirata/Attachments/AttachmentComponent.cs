namespace Content.Shared._EstacaoPirata.Attachments;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class AttachmentComponent : Component
{
    /// <summary>
    /// The actual item-slot that contains the cell. Allows all the interaction logic to be handled by <see cref="ItemSlotsSystem"/>.
    /// </summary>
    [DataField("attachmentSlotId", required: true)]
    public string AttachmentSlotId = string.Empty;
}
