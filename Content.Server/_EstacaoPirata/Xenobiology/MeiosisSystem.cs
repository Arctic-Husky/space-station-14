using System.Linq;
using Content.Shared._EstacaoPirata.Xenobiology.Meiosis;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeFeeding;
using Content.Shared.Mind;
using Robust.Server.GameObjects;
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

    public override void Initialize()
    {
        SubscribeLocalEvent<MeiosisComponent,SlimeTotallyFedEvent>(OnSlimeFed);
    }

    private void OnSlimeFed(EntityUid uid, MeiosisComponent component, SlimeTotallyFedEvent args)
    {
        DoMeiosis(args.Entity, component);
    }

    // TODO: criar HTN proprio do slime
    // TODO: um componente pra permitir analise pelo slime scanner que pega informacoes do meiosis component
    // TODO: fazer logging das coisas pra adm

    // TODO: lidar com a morte?
    private void DoMeiosis(EntityUid uid, MeiosisComponent meiosisComponent)
    {
        Log.Debug("Iniciando Meiose");
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
            var spawnedEntity = Spawn(entity, position);
            spawnedEntities.Add(spawnedEntity);
        }

        // This transfers the mind to the new entity
        if (_mindSystem.TryGetMind(uid, out var mindId, out var mind))
            _mindSystem.TransferTo(mindId, spawnedEntities.First(), mind: mind);

        // Deletar o original
        //QueueDel(uid);
    }
}
