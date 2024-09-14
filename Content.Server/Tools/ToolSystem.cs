using Content.Server.Atmos.EntitySystems;
<<<<<<< HEAD
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.FixedPoint;
using Content.Shared.Tools.Components;
using Robust.Server.GameObjects;
=======
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.Popups;
using Content.Server.Tools.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;

using SharedToolSystem = Content.Shared.Tools.Systems.SharedToolSystem;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

using SharedToolSystem = Content.Shared.Tools.Systems.SharedToolSystem;

namespace Content.Server.Tools;

public sealed class ToolSystem : SharedToolSystem
{
    [Dependency] private readonly AtmosphereSystem _atmosphereSystem = default!;
    [Dependency] private readonly TransformSystem _transformSystem = default!;

    public override void TurnOn(Entity<WelderComponent> entity, EntityUid? user)
    {
<<<<<<< HEAD
        base.TurnOn(entity, user);
        var xform = Transform(entity);
        if (xform.GridUid is { } gridUid)
        {
            var position = _transformSystem.GetGridOrMapTilePosition(entity.Owner, xform);
            _atmosphereSystem.HotspotExpose(gridUid, position, 700, 50, entity.Owner, true);
=======
        [Dependency] private readonly AtmosphereSystem _atmosphereSystem = default!;
        [Dependency] private readonly PopupSystem _popup = default!;
        [Dependency] private readonly SharedAudioSystem _audio = default!;
        [Dependency] private readonly SolutionContainerSystem _solutionContainer = default!;
        [Dependency] private readonly TransformSystem _transformSystem = default!;

        public override void Initialize()
        {
            base.Initialize();

            InitializeWelders();
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        }
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        UpdateWelders(frameTime);
    }

    //todo move to shared once you can remove reagents from shared without it freaking out.
    private void UpdateWelders(float frameTime)
    {
        var query = EntityQueryEnumerator<WelderComponent, SolutionContainerManagerComponent>();
        while (query.MoveNext(out var uid, out var welder, out var solutionContainer))
        {
            if (!welder.Enabled)
                continue;

            welder.WelderTimer += frameTime;

            if (welder.WelderTimer < welder.WelderUpdateTimer)
                continue;

            if (!SolutionContainerSystem.TryGetSolution((uid, solutionContainer), welder.FuelSolutionName, out var solutionComp, out var solution))
                continue;

            SolutionContainerSystem.RemoveReagent(solutionComp.Value, welder.FuelReagent, welder.FuelConsumption * welder.WelderTimer);

            if (solution.GetTotalPrototypeQuantity(welder.FuelReagent) <= FixedPoint2.Zero)
            {
                ItemToggle.Toggle(uid, predicted: false);
            }

            Dirty(uid, welder);
            welder.WelderTimer -= welder.WelderUpdateTimer;
        }
    }
}

