using System.Linq;
using Content.Shared._EstacaoPirata.Xenobiology.Meiosis;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeFeeding;
using Content.Shared.Mind;
using Content.Shared.Mobs.Systems;
using Robust.Server.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server._EstacaoPirata.Xenobiology;

/// <summary>
/// This handles...
/// </summary>
public sealed class MeiosisSystem : EntitySystem
{
    [Dependency] private readonly TransformSystem _transform = default!;
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    [Dependency] private readonly SharedMindSystem _mindSystem = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<MeiosisComponent,SlimeTotallyFedEvent>(OnSlimeFed);
    }

    private void OnSlimeFed(EntityUid uid, MeiosisComponent component, SlimeTotallyFedEvent args)
    {
        DoMeiosis(args.Entity, component);
    }

    // TODO: um componente pra permitir analise pelo slime scanner que pega informacoes do meiosis component
    // TODO: fazer logging das coisas pra adm

    // TODO: lidar com a morte?
    private void DoMeiosis(EntityUid uid, MeiosisComponent meiosisComponent)
    {
        Log.Debug("Iniciando Meiose");

        if (!_mobState.IsAlive(uid))
            return;

        var position = _transform.GetMapCoordinates(uid);

        var entitiesToSpawn = new List<string>();

        for (int i = 0; i < meiosisComponent.NumberOfBabies; i++)
        {
            var entity = meiosisComponent.Baby.Id;

            var randomNumber = _robustRandom.NextFloat();

            meiosisComponent.MutationSeverities.TryGetValue(meiosisComponent.MutationChance, out var percentages);

            var mutationChance = _robustRandom.NextFloat(percentages.Item1, percentages.Item2);

            Log.Debug($"Random Number: {randomNumber*100}% vs {mutationChance*100}% Mutation Chance");

            if (meiosisComponent.Mutations.Count > 0)
            {
                if (randomNumber <= mutationChance)
                {
                    var pick = _robustRandom.Pick(meiosisComponent.Mutations);
                    entity = pick.Id;
                }
            }

            entitiesToSpawn.Add(entity);
        }

        var spawnedEntities = new List<EntityUid>();

        foreach (var entity in entitiesToSpawn)
        {
            var randomizedPosition = position.Position;
            randomizedPosition.X = _robustRandom.NextFloat(randomizedPosition.X-0.25f, randomizedPosition.X+0.25f);
            randomizedPosition.Y = _robustRandom.NextFloat(randomizedPosition.Y-0.25f, randomizedPosition.Y+0.25f);
            var randomizedCoordinates = new MapCoordinates(randomizedPosition.X, randomizedPosition.Y, position.MapId);

            var spawnedEntity = Spawn(entity, randomizedCoordinates);
            spawnedEntities.Add(spawnedEntity);
        }

        // This transfers the mind to the new entity
        if (_mindSystem.TryGetMind(uid, out var mindId, out var mind))
            _mindSystem.TransferTo(mindId, spawnedEntities.First(), mind: mind);

        // Deletar o original
        //QueueDel(uid);
    }

    public T EnumNext<T>(T enumValue) where T : Enum {
        T[] enumValues = (T[])Enum.GetValues(typeof(T));
        int currentIndex = Array.IndexOf(enumValues, enumValue);
        int nextIndex = (currentIndex + 1) % enumValues.Length;
        return enumValues[nextIndex];
    }

    public T EnumPrevious<T>(T enumValue) where T : Enum {
        T[] enumValues = (T[])Enum.GetValues(typeof(T));
        int currentIndex = Array.IndexOf(enumValues, enumValue);
        int previousIndex = (currentIndex - 1 + enumValues.Length) % enumValues.Length;
        return enumValues[previousIndex];
    }
}
