using Content.Shared.Containers.ItemSlots;
using Content.Shared.Item;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server._EstacaoPirata.Xenobiology.ExtractScanner.Components;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class ExtractScannerComponent : Component
{
    /// <summary>
    /// The sound played when an artifact has points extracted.
    /// </summary>
    [DataField("sellSound")]
    public SoundSpecifier SellSound = new SoundPathSpecifier("/Audio/Machines/scan_finish.ogg");
}
