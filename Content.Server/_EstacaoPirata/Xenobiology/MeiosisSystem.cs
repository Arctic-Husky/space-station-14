using Content.Shared._EstacaoPirata.Xenobiology.Meiosis;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeFeeding;
using Content.Shared.Nutrition.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server._EstacaoPirata.Xenobiology;

/// <summary>
/// This handles...
/// </summary>
public sealed class MeiosisSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly TransformSystem _transform = default!;
    [Dependency] private readonly IRobustRandom _robustRandom = default!;

    public override void Initialize()
    {

    }

    // TODO: criar um componente pra lidar com o crescimento de um slime
    // TODO: criar HTN proprio do slime
    // TODO: um componente pra permitir analise pelo slime scanner que pega informacoes do meiosis component

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<MeiosisComponent, HungerComponent, SlimeFeedingComponent>();
        while (query.MoveNext(out var uid, out var meiosis, out var hunger, out var feeding))
        {
            if (_timing.CurTime < feeding.NextUpdateTime)
                continue;

            feeding.NextUpdateTime = _timing.CurTime + feeding.UpdateRate;

            if(hunger.CurrentThreshold < HungerThreshold.Okay)
                continue;

            if (feeding.LastHungerValue <= hunger.CurrentHunger)
            {
                var difference =  hunger.CurrentHunger - feeding.LastHungerValue;
                feeding.FeedingMeter += difference;

                if (feeding.FeedingMeter >= feeding.FeedingLimit)
                {
                    // Fazer meiose e deletar a entidade original
                    DoMeiosis(uid, meiosis);
                }
            }
            feeding.LastHungerValue = hunger.CurrentHunger;
        }
    }

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

            if (randomNumber <= mutationChance)
            {
                var pick = _robustRandom.Pick(meiosisComponent.Mutations);
                entity = pick.Id;
            }

            entitiesToSpawn.Add(entity);
        }

        foreach (var entity in entitiesToSpawn)
        {
            Spawn(entity, position);
        }

        // Deletar o original
        QueueDel(uid);
    }
}
