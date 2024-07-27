using Content.Shared._EstacaoPirata.Xenobiology.SlimeCrossbreeding;
using Content.Shared.Interaction.Events;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeCrossbreeding;

/// <summary>
/// This handles...
/// </summary>
public sealed class SlimeCrossbreedingSystem : EntitySystem
{
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<SlimeCrossbreedingComponent, GettingInteractedWithAttemptEvent>(OnGettingInteractedAttempt);
    }

    // Interaction with extracts
    private void OnGettingInteractedAttempt(Entity<SlimeCrossbreedingComponent> ent, ref GettingInteractedWithAttemptEvent args)
    {
        Log.Debug($"Interagido com {args.Target}");
    }
}
