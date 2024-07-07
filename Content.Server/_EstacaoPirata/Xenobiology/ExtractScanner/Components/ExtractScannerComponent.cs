using Robust.Shared.Audio;

namespace Content.Server._EstacaoPirata.Xenobiology.ExtractScanner.Components;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class ExtractScannerComponent : Component
{
    [DataField("sellSound")]
    public SoundSpecifier SellSound = new SoundPathSpecifier("/Audio/Machines/scan_finish.ogg");
}
