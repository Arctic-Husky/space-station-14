using Content.Server.Fluids.EntitySystems;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Coordinates.Helpers;
using Content.Shared.Maps;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;
using Serilog;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeReaction.Reactions;

public sealed partial class SpawnSmoke : SlimeReagentEffect
{
    /// <summary>
    /// Smoke entity to spawn.
    /// Defaults to smoke but you can use foam if you want.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<EntityPrototype> SmokePrototype = "Smoke";

    [DataField("sound")]
    public SoundSpecifier? Sound;

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

        var ent = args.EntityManager.Spawn(protoName:SmokePrototype.Id,coordinates:mapCoordinates);
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

        var spreadAmount = args.Quantity.Value / 10;

        var duration = Math.Log(args.Quantity.Value, 1.25);
        duration = Math.Round(duration);

        // TODO: a solucao do extract sempre vai chegar aqui vazia, tentar ver um jeito de fazer ela nao estar vazia pra poder adicionar reagentes na fumaca
        smokeSystem.StartSmoke(ent, soln.Value.Comp.Solution, (float)duration, spreadAmount, smokeComp);
        var audioSystem = args.EntityManager.System<SharedAudioSystem>();

        var sound = Sound == null ? args.ReactionComponent.ReactionSound : Sound;

        PlaySound(audioSystem, sound, extractEntity);

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

    public override float NeedsTime()
    {
        return 0f;
    }

    public override void PlaySound(SharedAudioSystem audioSystem, SoundSpecifier? sound, EntityUid entity)
    {
        audioSystem.PlayPvs(sound, entity);
    }
}
