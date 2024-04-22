using Content.Shared._EstacaoPirata.Xenobiology.SlimeFeeding;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeGrowth;
using Content.Shared.Mind;
using Robust.Server.GameObjects;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeGrowth;

/// <summary>
/// This handles the growth of a baby slime to adulthood.
/// </summary>
public sealed class SlimeGrowthSystem : EntitySystem
{
    [Dependency] private readonly TransformSystem _transform = default!;
    [Dependency] private readonly SharedMindSystem _mindSystem = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<SlimeGrowthComponent, SlimeTotallyFedEvent>(OnSlimeFed);
    }

    private void OnSlimeFed(EntityUid uid, SlimeGrowthComponent component, SlimeTotallyFedEvent args)
    {
        DoGrowth(args.Entity, component);
    }

    private void DoGrowth(EntityUid entity, SlimeGrowthComponent component)
    {
        Log.Debug($"Iniciando Growth");

        var position = _transform.GetMapCoordinates(entity);

        var spawnedEntity = Spawn(component.Adult, position);

        // This transfers the mind to the new entity
        if (_mindSystem.TryGetMind(entity, out var mindId, out var mind))
            _mindSystem.TransferTo(mindId, spawnedEntity, mind: mind);
    }
}
