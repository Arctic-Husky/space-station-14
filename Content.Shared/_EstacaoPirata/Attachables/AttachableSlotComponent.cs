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
}
