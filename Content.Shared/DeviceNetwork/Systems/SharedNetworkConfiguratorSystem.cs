using Content.Shared.Actions;
<<<<<<< HEAD
using Content.Shared.DeviceNetwork.Components;
using Content.Shared.UserInterface;
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
using Robust.Shared.Serialization;

namespace Content.Shared.DeviceNetwork.Systems;

public abstract class SharedNetworkConfiguratorSystem : EntitySystem
{
<<<<<<< HEAD
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<NetworkConfiguratorComponent, ActivatableUIOpenAttemptEvent>(OnUiOpenAttempt);
    }

    private void OnUiOpenAttempt(EntityUid uid, NetworkConfiguratorComponent configurator, ActivatableUIOpenAttemptEvent args)
    {
        if (configurator.LinkModeActive)
            args.Cancel();
    }
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
}

public sealed partial class ClearAllOverlaysEvent : InstantActionEvent
{
}

[Serializable, NetSerializable]
public enum NetworkConfiguratorVisuals
{
    Mode
}

[Serializable, NetSerializable]
public enum NetworkConfiguratorLayers
{
    ModeLight
}
