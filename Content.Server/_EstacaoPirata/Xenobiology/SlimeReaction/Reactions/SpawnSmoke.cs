using Content.Server.Fluids.EntitySystems;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Coordinates.Helpers;
using Content.Shared.Maps;
using Robust.Server.GameObjects;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeReaction.Reactions;

public sealed partial class SpawnSmoke : SlimeReagentEffect
{
    /// <summary>
    /// Smoke entity to spawn.
    /// Defaults to smoke but you can use foam if you want.
    /// </summary>
    [DataField("prototype"), ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<EntityPrototype> Prototype = "Smoke";

    public override bool Effect(SlimeReagentEffectArgs args)
    {
        var transformSystem = args.EntityManager.System<TransformSystem>();
        var smokeSystem = args.EntityManager.System<SmokeSystem>();
        var mapSystem = args.EntityManager.System<MapSystem>();

        var extractEntity = args.ExtractEntity;

        if (!args.EntityManager.TryGetComponent<TransformComponent>(extractEntity, out var xform))
        {
            return false;
        }

        var mapCoordinates = transformSystem.GetMapCoordinates(extractEntity);

        var entityCoordinates = transformSystem.GetMoverCoordinates(extractEntity);

        var foundGrid = transformSystem.TryGetGridTilePosition(extractEntity, out var vector);


        if (!foundGrid)
        {
            return false;
        }

        if (xform.GridUid == null)
            return false;

        if(!args.EntityManager.TryGetComponent<MapGridComponent>(xform.GridUid.Value, out var gridComp))
        {
            return false;
        }

        mapSystem.TryGetTileRef(extractEntity, gridComp, vector, out var tileRef);

        if (tileRef.Tile.IsSpace())
        {
            return false;
        }

        var ent = args.EntityManager.Spawn(protoName:Prototype.Id,coordinates:mapCoordinates);
        entityCoordinates.SnapToGrid();

        if (!args.EntityManager.HasComponent<SmokeComponent>(ent))
        {
            args.EntityManager.DeleteEntity(ent);
            return false;
        }

        if (!GetSolution(args, extractEntity, out var soln))
        {
            args.EntityManager.DeleteEntity(ent);
            return false;
        }


        if (!soln.HasValue)
        {
            args.EntityManager.DeleteEntity(ent);
            return false;
        }


        var smokeComp = args.EntityManager.GetComponent<SmokeComponent>(ent);

        // TODO: ver sobre esses valores hardcoded

        var allReagentsQuantity = soln.Value.Comp.Solution.Volume.Value / 100;

        var catalystQuantity = args.Quantity.Value / 100;

        var range = (int) MathF.Round(MathHelper.Lerp(catalystQuantity, allReagentsQuantity+10, args.RobustRandom.NextFloat(0, 1f)));

        var duration = Math.Log(args.Quantity.Value, 1.25);
        duration = Math.Round(duration);

        smokeSystem.StartSmoke(ent, soln.Value.Comp.Solution, (float)duration, range, smokeComp);

        return true;
    }

    private bool GetSolution(SlimeReagentEffectArgs args, EntityUid extractEntity, out Entity<SolutionComponent>? soln)
    {
        var solutionContainerSystem = args.EntityManager.System<SharedSolutionContainerSystem>();
        var solutionContainerManagerComponent = args.EntityManager.GetComponent<SolutionContainerManagerComponent>(extractEntity);
        var slimeReactionComponent = args.EntityManager.GetComponent<SlimeReactionComponent>(extractEntity);

        var entity = new Entity<SolutionContainerManagerComponent?>(extractEntity, solutionContainerManagerComponent);

        if (!solutionContainerSystem.TryGetSolution(entity, slimeReactionComponent.SolutionName, out soln))
            return false;
        return true;
    }

    public override float TimeNeeded()
    {
        return 0f;
    }

    public override bool SpendOnUse()
    {
        return false;
    }

    public override string GetReactionMessage()
    {
        return "extract-smoke";
    }
}
