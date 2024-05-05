using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction.Reactions;

public sealed partial class SpawnEntity : SlimeReagentEffect
{
    // TODO: muar isto para uma lista de strings
    [DataField("toSpawn")]
    public Dictionary<string, int> ToSpawn;

    public override bool Effect(SlimeReagentEffectArgs args)
    {
        // Com os args, que deve ser algo tipo target e outras coisas, fazer o codigo funcionar por aqui, pra nao precisar definir tudo no
        // system de SlimeReaction. Quero apenas chamar em slimeReaction o SlimeReagentEffect.Effect() pra funcionar automatico plss

        var extractEntity = args.ExtractEntity;

        foreach (var prototype in ToSpawn)
        {
            for (int i = 0; i < prototype.Value; i++)
            {
                var transformSystem = args.EntityManager.System<SharedTransformSystem>();
                var coordinates = transformSystem.GetMapCoordinates(extractEntity);
                var randomizedPosition = coordinates.Position;

                randomizedPosition.X = args.RobustRandom.NextFloat(randomizedPosition.X-0.25f, randomizedPosition.X+0.25f);
                randomizedPosition.Y = args.RobustRandom.NextFloat(randomizedPosition.Y-0.25f, randomizedPosition.Y+0.25f);

                var randomizedCoordinates = new MapCoordinates(randomizedPosition.X, randomizedPosition.Y, coordinates.MapId);

                args.EntityManager.Spawn(prototype.Key, randomizedCoordinates);
            }
        }

        var audioSystem = args.EntityManager.System<SharedAudioSystem>();

        PlaySound(audioSystem, args.ReactionComponent.ReactionSound, args.ExtractEntity);

        return true;
    }

    public override float TimeNeeded()
    {
        return 0;
    }

    public override bool SpendOnUse()
    {
        return true;
    }

    public override void PlaySound(SharedAudioSystem audioSystem, SoundSpecifier? sound, EntityUid entity)
    {
        audioSystem.PlayPvs(sound, entity);
    }
}
