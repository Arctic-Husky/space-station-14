using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Xenoarchaeology.Equipment;

[Serializable, NetSerializable]
public enum ArtifactAnalzyerUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class AnalysisConsoleServerSelectionMessage : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class AnalysisConsoleScanButtonPressedMessage : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class AnalysisConsolePrintButtonPressedMessage : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class AnalysisConsoleExtractButtonPressedMessage : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class AnalysisConsoleBiasButtonPressedMessage(bool isDown) : BoundUserInterfaceMessage
{
<<<<<<< HEAD
    public bool IsDown = isDown;
}

[Serializable, NetSerializable]
public sealed class AnalysisConsoleUpdateState(
    NetEntity? artifact,
    bool analyzerConnected,
    bool serverConnected,
    bool canScan,
    bool canPrint,
    FormattedMessage? scanReport,
    bool scanning,
    bool paused,
    TimeSpan? startTime,
    TimeSpan? accumulatedRunTime,
    TimeSpan? totalTime,
    int pointAmount,
    bool isTraversalDown
)
    : BoundUserInterfaceState
{
    public NetEntity? Artifact = artifact;
    public bool AnalyzerConnected = analyzerConnected;
    public bool ServerConnected = serverConnected;
    public bool CanScan = canScan;
    public bool CanPrint = canPrint;
    public FormattedMessage? ScanReport = scanReport;
    public bool Scanning = scanning;
    public bool Paused = paused;
    public TimeSpan? StartTime = startTime;
    public TimeSpan? AccumulatedRunTime = accumulatedRunTime;
    public TimeSpan? TotalTime = totalTime;
    public int PointAmount = pointAmount;
    public bool IsTraversalDown = isTraversalDown;
=======
    public NetEntity? Artifact;

    public bool AnalyzerConnected;

    public bool ServerConnected;

    public bool CanScan;

    public bool CanPrint;

    public FormattedMessage? ScanReport;

    public bool Scanning;

    public bool Paused;

    public TimeSpan? StartTime;

    public TimeSpan? AccumulatedRunTime;

    public TimeSpan? TotalTime;

    public int PointAmount;

    public AnalysisConsoleScanUpdateState(NetEntity? artifact, bool analyzerConnected, bool serverConnected, bool canScan, bool canPrint,
        FormattedMessage? scanReport, bool scanning, bool paused, TimeSpan? startTime, TimeSpan? accumulatedRunTime, TimeSpan? totalTime, int pointAmount)
    {
        Artifact = artifact;
        AnalyzerConnected = analyzerConnected;
        ServerConnected = serverConnected;
        CanScan = canScan;
        CanPrint = canPrint;

        ScanReport = scanReport;

        Scanning = scanning;
        Paused = paused;

        StartTime = startTime;
        AccumulatedRunTime = accumulatedRunTime;
        TotalTime = totalTime;

        PointAmount = pointAmount;
    }
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
}
