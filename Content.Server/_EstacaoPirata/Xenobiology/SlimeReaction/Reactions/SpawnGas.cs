using Content.Server.Atmos;
using Content.Server.Atmos.EntitySystems;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Shared.Atmos;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeReaction.Reactions;

public sealed partial class SpawnGas : SlimeReagentEffect
{
    [DataField("spawnGas")]
    public Gas? toSpawn { get; set; } = null;

    [DataField("spawnTemperature")]
    public float SpawnTemperature { get; set; } = Atmospherics.FireMinimumTemperatureToSpread;

    [DataField("spawnAmount")]
    public float SpawnAmount { get; set; } = Atmospherics.OxygenMolesStandard;

    [DataField("ignite")]
    public bool Ignite { get; set; } = false;

    [DataField("delayTime")]
    public float Time { get; set; } = 0;

    public override bool Effect(SlimeReagentEffectArgs args)
    {
        if (toSpawn == null)
            return false;

        var extractEntity = args.ExtractEntity;

        var atmosphereSystem = args.EntityManager.System<AtmosphereSystem>();

        var transform = args.EntityManager.GetComponent<TransformComponent>(extractEntity);

        var transformSystem = args.EntityManager.System<TransformSystem>();

        var environment = atmosphereSystem.GetContainingMixture((extractEntity, transform), true, true);

        if (environment == null)
            return false;

        var merger = new GasMixture(1) { Temperature = SpawnTemperature };
        merger.SetMoles(toSpawn.Value, SpawnAmount);

        atmosphereSystem.Merge(environment, merger);

        if (Ignite)
        {
            if (transform.GridUid is { } gridUid)
            {
                var position = transformSystem.GetGridOrMapTilePosition(args.ExtractEntity, transform);
                atmosphereSystem.HotspotExpose(gridUid, position, 700, 50);
            }

        }

        return true;
    }

    public override float TimeNeeded()
    {
        return Time;
    }

    public override bool SpendOnUse()
    {
        return false;
    }

    public override void PlaySound(SharedAudioSystem audioSystem, SoundSpecifier? sound, EntityUid entity)
    {
        audioSystem.PlayPvs(sound, entity);
    }
}
