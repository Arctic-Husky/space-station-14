using System.Linq;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeCrossbreeding;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Shared.Interaction;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeCrossbreeding;

/// <summary>
/// This handles...
/// </summary>
public sealed class SlimeCrossbreedingSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<SlimeCrossbreedingComponent, InteractUsingEvent>(OnInteraction);
    }

    // Interaction for crossbreeding
    private void OnInteraction(Entity<SlimeCrossbreedingComponent> ent, ref InteractUsingEvent args)
    {
        // verificar se o slime esta morto?

        if (!TryComp<SlimeReactionComponent>(args.Used, out var extract))
            return;

        if (!TryComp<SlimeCrossbreedingComponent>(args.Target, out var target))
            return;

        if (target.Crossbreeds is null)
            return;

        // Extract.Color pode ser vazio, lidar com isso

        if (!target.Crossbreeds.TryGetValue(extract.Color, out var value)) // Ja obtem o entry da cor certa
            return;

        if (!ent.Comp.ExtractsUsed.TryAdd(extract.Color, 1))
        {
            if (ent.Comp.ExtractsUsed.TryGetValue(extract.Color, out var timesUsed))
            {
                ent.Comp.ExtractsUsed[extract.Color] = timesUsed + 1;
                if (timesUsed >= ent.Comp.Max)
                {
                    ent.Comp.MaxAchieved = true;
                    // ent.Comp.MaxColor = extract.Color;
                }
            }
        }

        if (ent.Comp.MaxAchieved)
        {
            var coords = _transform.GetMapCoordinates(args.Target);

            var entity = Spawn(value.Prototype, coords);

            SlimeReactionComponent component = new SlimeReactionComponent
            {
                Reactions = value.Reactions
            };

            AddComp(entity, component);

            QueueDel(args.Target);
        }

        QueueDel(args.Used);

        // Log.Debug($"Interagido com {args.Target}");

        // var coords = _transform.GetMapCoordinates(args.Target);

        // var entity = Spawn(value.Prototype, coords);

        // var component = new SlimeReactionComponent
        // {
        //     Reactions = value.Reactions
        // };
        //
        // AddComp(entity, component);
    }
}
