using Robust.Shared.Serialization;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeGrinder;


[Serializable, NetSerializable]
public sealed class SlimeGrinderStartGrindingMessage : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class SlimeGrinderStopGrindingMessage : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class SlimeGrinderEjectSolidIndexedMessage : BoundUserInterfaceMessage
{
    public NetEntity EntityID;
    public SlimeGrinderEjectSolidIndexedMessage(NetEntity entityId)
    {
        EntityID = entityId;
    }
}

[NetSerializable, Serializable]
public sealed class SlimeGrinderUpdateUserInterfaceState : BoundUserInterfaceState
{
    public NetEntity[] ContainedSolids;
    public bool IsBusy;
    //public int ActiveButtonIndex;

    public SlimeGrinderUpdateUserInterfaceState(NetEntity[] containedSolids,
        bool isBusy) //, int activeButtonIndex
    {
        ContainedSolids = containedSolids;
        IsBusy = isBusy;
        //ActiveButtonIndex = activeButtonIndex;
    }

}

[Serializable, NetSerializable]
public enum SlimeGrinderVisualState : byte
{
    Idle,
    Grinding
}

[Serializable, NetSerializable]
public enum SlimeGrinderVisualLayers : byte
{
    Base
}

[Serializable, NetSerializable]
public enum SlimeGrinderVisuals : byte
{
    VisualState
}

[NetSerializable, Serializable]
public enum SlimeGrinderUiKey
{
    Key
}
