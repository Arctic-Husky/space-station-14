using Content.Server.Atmos;
using Content.Server.Atmos.EntitySystems;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Shared.Atmos;
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

    [DataField("hotspot")]
    public bool Hotspot { get; set; } = true;

    [DataField("delayTime")]
    public float Time { get; set; } = 0;

    public override bool Effect(SlimeReagentEffectArgs args)
    {
        if (toSpawn == null)
            return false;

        var extractEntity = args.ExtractEntity;

        var atmosphereSystem = args.EntityManager.System<AtmosphereSystem>();

        var transform = args.EntityManager.GetComponent<TransformComponent>(extractEntity);

        var environment = atmosphereSystem.GetContainingMixture((extractEntity, transform), true, true);

        if (environment == null)
            return false;

        var merger = new GasMixture(1) { Temperature = SpawnTemperature };
        merger.SetMoles(toSpawn.Value, SpawnAmount);

        atmosphereSystem.Merge(environment, merger);

        return true;
    }

    public override float NeedsTime()
    {
        return Time;
    }

    public override void PlaySound(SharedAudioSystem audioSystem, SoundSpecifier? sound, EntityUid entity)
    {
        audioSystem.PlayPvs(sound, entity);
    }
}
