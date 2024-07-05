using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared._EstacaoPirata.Xenobiology.ExtractScanner;

[Serializable, NetSerializable]
public sealed class ExtractSellMessage : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class ExtractScannerEjectMessage : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class ExtractScannerServerSelectionMessage : BoundUserInterfaceMessage
{
}


[NetSerializable, Serializable]
public sealed class ExtractScannerUpdateState(
        NetEntity? containedSolid,
        FormattedMessage? extractInfo,
        bool canSell,
        bool serverConnected
    )
    : BoundUserInterfaceState
{
    public NetEntity? ContainedSolid = containedSolid;
    //public bool CanScan = canScan;
    public bool ServerConnected = serverConnected;
    public bool CanSell = canSell;
    public FormattedMessage? ExtractInfo = extractInfo;

    // public ExtractScannerUpdateState(NetEntity? containedSolid,
    //     bool canScan)
    // {
    //     ContainedSolid = containedSolid;
    //     CanScan = canScan;
    // }
}

[Serializable, NetSerializable]
public enum ExtractScannerVisualState : byte
{
    Scanning
}

[Serializable, NetSerializable]
public enum ExtractScannerVisualizerLayers : byte
{
    Base,
    BaseUnlit
}

[NetSerializable, Serializable]
public enum ExtractScannerUiKey
{
    Key
}
