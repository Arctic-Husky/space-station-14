using Content.Server.Atmos;
using Content.Server.Atmos.EntitySystems;
using Content.Server.IgnitionSource;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Shared.Atmos;
using Robust.Server.GameObjects;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeReaction.Reactions;

public sealed partial class SpawnGas : SlimeReagentEffect
{
    [DataField("spawnGas")]
    public Gas? toSpawn { get; set; } = null;

    [DataField("spawnTemperature")]
    public float SpawnTemperature { get; set; } = Atmospherics.FireMinimumTemperatureToSpread;

    [DataField("spawnAmount")]
    public float SpawnAmount { get; set; } = Atmospherics.OxygenMolesStandard;

    [DataField("hotspot")]
    public bool Hotspot { get; set; } = true;

    [DataField("delayTime")]
    public float Time { get; set; } = 0;

    public override bool Effect(SlimeReagentEffectArgs args)
    {
        if (toSpawn == null)
            return false;

        if (args.ExtractEntity == null)
            return false;

        var atmosphereSystem = args.EntityManager.System<AtmosphereSystem>();

        var transform = args.EntityManager.GetComponent<TransformComponent>(args.ExtractEntity.Value);

        var environment = atmosphereSystem.GetContainingMixture((args.ExtractEntity.Value, transform), true, true);

        if (environment == null)
            return false;

        var merger = new GasMixture(1) { Temperature = SpawnTemperature };
        merger.SetMoles(toSpawn.Value, SpawnAmount);

        atmosphereSystem.Merge(environment, merger);

        args.EntityManager.RemoveComponentDeferred<ActiveSlimeReactionComponent>(args.ExtractEntity.Value);

        // var ignitionSourceComponent = args.EntityManager.AddComponent<IgnitionSourceComponent>(args.ExtractEntity.Value);
        //
        // var ignitionSourceSystem = args.EntityManager.System<IgnitionSourceSystem>();
        //
        // ignitionSourceSystem.SetIgnited(args.ExtractEntity.Value, true);

        return true;
    }

    public override float NeedsTime()
    {
        return Time;
    }
}
