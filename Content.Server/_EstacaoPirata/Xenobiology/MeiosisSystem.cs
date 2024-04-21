using Content.Shared._EstacaoPirata.Xenobiology.Meiosis;
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
    [Dependency] private readonly IRobustRandom RandMan = default!;

    public override void Initialize()
    {

    }

    // TODO: criar um componente pra lidar com o crescimento de um slime

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<MeiosisComponent, HungerComponent>();
        while (query.MoveNext(out var uid, out var meiosis, out var hunger))
        {
            if (_timing.CurTime < meiosis.NextUpdateTime)
                continue;

            meiosis.NextUpdateTime = _timing.CurTime + meiosis.UpdateRate;

            if(hunger.CurrentThreshold < HungerThreshold.Okay)
                continue;

            if (meiosis.LastHungerValue <= hunger.CurrentHunger)
            {
                var difference =  hunger.CurrentHunger - meiosis.LastHungerValue;
                meiosis.FeedingMeter += difference;

                if (meiosis.FeedingMeter >= meiosis.FeedingLimit)
                {
                    // Fazer meiose
                    DoMeiosis(uid, meiosis);

                    meiosis.FeedingMeter = 0;
                }
            }
            meiosis.LastHungerValue = hunger.CurrentHunger;
        }
    }

    private void DoMeiosis(EntityUid uid, MeiosisComponent meiosisComponent)
    {
        //We're empty. Become trash.
        var position = _transform.GetMapCoordinates(uid);

        //RandMan.NextByte((byte)meiosisComponent.NumberOfBabies);

        var entitiesToSpawn = new List<string>();

        for (int i = 0; i < meiosisComponent.NumberOfBabies; i++)
        {
            var entity = meiosisComponent.Baby.Id;

            var result = RandMan.NextFloat();

            if (result <= meiosisComponent.MutationChance)
            {
                var pick = RandMan.Pick(meiosisComponent.Neighbors);
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
