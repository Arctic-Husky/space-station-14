using Content.Shared.Stacks;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction.Reactions;

public sealed partial class SpawnEntity : SlimeReagentEffect
{
    // TODO: muar isto para uma lista de strings
    [DataField("toSpawn")]
    public Dictionary<string, int> ToSpawn;

    /// <summary>
    /// Won't work if the entity doesn't have the stack component
    /// </summary>
    [DataField("stack")]
    public bool Stack = false;

    public override bool Effect(SlimeReagentEffectArgs args)
    {
        var extractEntity = args.ExtractEntity;

        var transformSystem = args.EntityManager.System<SharedTransformSystem>();

        var stackSystem = args.EntityManager.System<SharedStackSystem>();

        foreach (var prototype in ToSpawn)
        {
            if (Stack)
            {
                var randomizedCoordinates = RandomizedCoordinates(args, transformSystem, extractEntity);

                var spawned = args.EntityManager.Spawn(prototype.Key, randomizedCoordinates);

                args.EntityManager.TryGetComponent<StackComponent>(spawned, out var stack);

                stackSystem.SetCount(spawned, prototype.Value, stack);
            }
            else
            {
                for (int i = 0; i < prototype.Value; i++)
                {
                    var randomizedCoordinates = RandomizedCoordinates(args, transformSystem, extractEntity);

                    args.EntityManager.Spawn(prototype.Key, randomizedCoordinates);
                }
            }

        }

        return true;
    }

    private MapCoordinates RandomizedCoordinates(SlimeReagentEffectArgs args, SharedTransformSystem transformSystem,
        EntityUid extractEntity)
    {
        var coordinates = transformSystem.GetMapCoordinates(extractEntity);
        var randomizedPosition = coordinates.Position;

        randomizedPosition.X = args.RobustRandom.NextFloat(randomizedPosition.X-0.25f, randomizedPosition.X+0.25f);
        randomizedPosition.Y = args.RobustRandom.NextFloat(randomizedPosition.Y-0.25f, randomizedPosition.Y+0.25f);

        var randomizedCoordinates = new MapCoordinates(randomizedPosition.X, randomizedPosition.Y, coordinates.MapId);
        return randomizedCoordinates;
    }

    public override float TimeNeeded()
    {
        return 0;
    }

    public override bool SpendOnUse()
    {
        return true;
    }

    public override string GetReactionMessage()
    {
        return "extract-entity";
    }
}
