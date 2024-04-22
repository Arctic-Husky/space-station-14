using Content.Shared._EstacaoPirata.Xenobiology.Meiosis;
using Robust.Shared.Prototypes;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeFeeding;

/// <summary>
/// This is used for controlling the feeding of the slime for the process of meiosis
/// </summary>
[RegisterComponent]
public sealed partial class SlimeFeedingComponent : Component
{
    /// <summary>
    /// Maximum distance between slime and target for it to feed on it
    /// </summary>
    [DataField("maxFeedingDistance"), ViewVariables]
    public float MaxFeedingDistance = 1f;

    /// <summary>
    /// This controls when the entity will enter the meiosis process
    /// </summary>
    [DataField("feedingMeter"), ViewVariables]
    public float FeedingMeter = 0f;

    /// <summary>
    /// The limit of how fed the entity needs to be to enter the meiosis process
    /// </summary>
    [DataField("feedingThreshold"), ViewVariables]
    public float FeedingLimit = 50f;

    [ViewVariables]
    public float LastHungerValue = 0f;

    /// <summary>
    /// The time when the hunger will update next.
    /// </summary>
    [DataField("nextUpdateTime"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan NextUpdateTime;

    /// <summary>
    /// The time between each update.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan UpdateRate = TimeSpan.FromSeconds(1);

    public ProtoId<EntityPrototype> SlimeNutrimentPrototype = "SlimeNutriment";
}

public sealed class SlimeTotallyFedEvent : EntityEventArgs
{
    public EntityUid Entity;

    public SlimeTotallyFedEvent(EntityUid entity)
    {
        Entity = entity;
    }
}
