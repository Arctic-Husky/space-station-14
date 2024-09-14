using System.Numerics;
using Content.Shared.Camera;
using Content.Shared.CCVar;
<<<<<<< HEAD
=======
using Content.Shared.Contests;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
using Robust.Shared.Configuration;

namespace Content.Client.Camera;

public sealed class CameraRecoilSystem : SharedCameraRecoilSystem
{
    [Dependency] private readonly IConfigurationManager _configManager = default!;
<<<<<<< HEAD
=======
    [Dependency] private readonly ContestsSystem _contests = default!;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    private float _intensity;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<CameraKickEvent>(OnCameraKick);

        Subs.CVar(_configManager, CCVars.ScreenShakeIntensity, OnCvarChanged, true);
    }

    private void OnCvarChanged(float value)
    {
        _intensity = value;
    }

    private void OnCameraKick(CameraKickEvent ev)
    {
        KickCamera(GetEntity(ev.NetEntity), ev.Recoil);
    }

    public override void KickCamera(EntityUid uid, Vector2 recoil, CameraRecoilComponent? component = null)
    {
        if (_intensity == 0)
            return;

        if (!Resolve(uid, ref component, false))
            return;

<<<<<<< HEAD
        recoil *= _intensity;

        // Use really bad math to "dampen" kicks when we're already kicked.
        var existing = component.CurrentKick.Length();
        var dampen = existing / KickMagnitudeMax;
        component.CurrentKick += recoil * (1 - dampen);
=======
        var massRatio = _contests.MassContest(uid);
        var maxRecoil = KickMagnitudeMax / massRatio;
        recoil *= _intensity / massRatio;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

        var existing = component.CurrentKick.Length();
        component.CurrentKick += recoil * (1 - existing);

        if (component.CurrentKick.Length() > maxRecoil)
            component.CurrentKick = component.CurrentKick.Normalized() * maxRecoil;

        component.LastKickTime = 0;
    }
}
