using Content.Server.DeviceNetwork.Components;
<<<<<<< HEAD
=======
using Content.Server.DeviceNetwork.Systems;
using Content.Server.Medical.CrewMonitoring;
using Content.Server.Medical.SuitSensors;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
using Content.Server.Popups;
using Content.Server.Power.EntitySystems;
using Content.Server.PowerCell;
using Content.Server.Radio.Components;
<<<<<<< HEAD
=======
using Content.Server.Station.Systems;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
using Content.Shared.DeviceNetwork.Components;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.PowerCell.Components;
using Content.Shared.RadioJammer;
using Content.Shared.Radio.EntitySystems;

namespace Content.Server.Radio.EntitySystems;

public sealed class JammerSystem : SharedJammerSystem
{
    [Dependency] private readonly PowerCellSystem _powerCell = default!;
    [Dependency] private readonly BatterySystem _battery = default!;
<<<<<<< HEAD
=======
    [Dependency] private readonly PopupSystem _popup = default!;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly StationSystem _stationSystem = default!;
    [Dependency] private readonly SingletonDeviceNetServerSystem _singletonServerSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RadioJammerComponent, ActivateInWorldEvent>(OnActivate);
        SubscribeLocalEvent<ActiveRadioJammerComponent, PowerCellChangedEvent>(OnPowerCellChanged);
        SubscribeLocalEvent<RadioJammerComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<RadioSendAttemptEvent>(OnRadioSendAttempt);
        SubscribeLocalEvent<SuitSensorComponent, SuitSensorsSendAttemptEvent>(OnSensorSendAttempt);
    }

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<ActiveRadioJammerComponent, RadioJammerComponent>();

        while (query.MoveNext(out var uid, out var _, out var jam))
        {
<<<<<<< HEAD

            if (_powerCell.TryGetBatteryFromSlot(uid, out var batteryUid, out var battery))
            {
                if (!_battery.TryUseCharge(batteryUid.Value, GetCurrentWattage(jam) * frameTime, battery))
                {
                    ChangeLEDState(false, uid);
                    RemComp<ActiveRadioJammerComponent>(uid);
                    RemComp<DeviceNetworkJammerComponent>(uid);
                }
                else
                {
                    var percentCharged = battery.CurrentCharge / battery.MaxCharge;
                    if (percentCharged > .50)
                    {
                        ChangeChargeLevel(RadioJammerChargeLevel.High, uid);
                    }
                    else if (percentCharged < .15)
                    {
                        ChangeChargeLevel(RadioJammerChargeLevel.Low, uid);
                    }
                    else
                    {
                        ChangeChargeLevel(RadioJammerChargeLevel.Medium, uid);
                    }
                }

=======
            if (_powerCell.TryGetBatteryFromSlot(uid, out var batteryUid, out var battery) &&
                !_battery.TryUseCharge(batteryUid.Value, jam.Wattage * frameTime, battery))
            {
                RemComp<ActiveRadioJammerComponent>(uid);
                RemComp<DeviceNetworkJammerComponent>(uid);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            }

        }
    }

    private void OnActivate(EntityUid uid, RadioJammerComponent comp, ActivateInWorldEvent args)
    {
        var activated = !HasComp<ActiveRadioJammerComponent>(uid) &&
            _powerCell.TryGetBatteryFromSlot(uid, out var battery) &&
            battery.CurrentCharge > GetCurrentWattage(comp);
        if (activated)
        {
            ChangeLEDState(true, uid);
            EnsureComp<ActiveRadioJammerComponent>(uid);
            EnsureComp<DeviceNetworkJammerComponent>(uid, out var jammingComp);
<<<<<<< HEAD
            jammingComp.Range = GetCurrentRange(comp);
=======
            jammingComp.Range = comp.Range;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            jammingComp.JammableNetworks.Add(DeviceNetworkComponent.DeviceNetIdDefaults.Wireless.ToString());
            Dirty(uid, jammingComp);
        }
        else
        {
<<<<<<< HEAD
            ChangeLEDState(false, uid);
            RemCompDeferred<ActiveRadioJammerComponent>(uid);
            RemCompDeferred<DeviceNetworkJammerComponent>(uid);
=======
            RemComp<ActiveRadioJammerComponent>(uid);
            RemComp<DeviceNetworkJammerComponent>(uid);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        }
        var state = Loc.GetString(activated ? "radio-jammer-component-on-state" : "radio-jammer-component-off-state");
        var message = Loc.GetString("radio-jammer-component-on-use", ("state", state));
        Popup.PopupEntity(message, args.User, args.User);
        args.Handled = true;
    }

    private void OnPowerCellChanged(EntityUid uid, ActiveRadioJammerComponent comp, PowerCellChangedEvent args)
    {
        if (args.Ejected)
        {
            ChangeLEDState(false, uid);
            RemCompDeferred<ActiveRadioJammerComponent>(uid);
        }
    }

    private void OnExamine(EntityUid uid, RadioJammerComponent comp, ExaminedEvent args)
    {
        if (args.IsInDetailsRange)
        {
            var powerIndicator = HasComp<ActiveRadioJammerComponent>(uid)
                ? Loc.GetString("radio-jammer-component-examine-on-state")
                : Loc.GetString("radio-jammer-component-examine-off-state");
            args.PushMarkup(powerIndicator);

            var powerLevel = Loc.GetString(comp.Settings[comp.SelectedPowerLevel].Name);
            var switchIndicator = Loc.GetString("radio-jammer-component-switch-setting", ("powerLevel", powerLevel));
            args.PushMarkup(switchIndicator);
        }
    }

    private void OnRadioSendAttempt(ref RadioSendAttemptEvent args)
    {
        if (ShouldCancelSend(args.RadioSource))
<<<<<<< HEAD
        {
            args.Cancelled = true;
=======
        {
            args.Cancelled = true;
        }
    }

    private void OnSensorSendAttempt(EntityUid uid, SuitSensorComponent comp, ref SuitSensorsSendAttemptEvent args)
    {
        if (ShouldCancelSend(uid))
        {
            args.Cancelled = true;
        }
    }

    private bool ShouldCancelSend(EntityUid sourceUid)
    {
        var source = Transform(sourceUid).Coordinates;
        var query = EntityQueryEnumerator<ActiveRadioJammerComponent, RadioJammerComponent, TransformComponent>();

        while (query.MoveNext(out _, out _, out var jam, out var transform))
        {
            if (source.InRange(EntityManager, _transform, transform.Coordinates, jam.Range))
            {
                return true;
            }
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        }

        return false;
    }

    private bool ShouldCancelSend(EntityUid sourceUid)
    {
        var source = Transform(sourceUid).Coordinates;
        var query = EntityQueryEnumerator<ActiveRadioJammerComponent, RadioJammerComponent, TransformComponent>();

        while (query.MoveNext(out _, out _, out var jam, out var transform))
        {
            if (source.InRange(EntityManager, _transform, transform.Coordinates, GetCurrentRange(jam)))
            {
                return true;
            }
        }

        return false;
    }
}
