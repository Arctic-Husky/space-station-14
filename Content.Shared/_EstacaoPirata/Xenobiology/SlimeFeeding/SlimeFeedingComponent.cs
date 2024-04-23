using Content.Shared.Damage;
using Content.Shared.DoAfter;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

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
    public float FeedingLimit = 100f;

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

    /// <summary>
    ///
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan LatchOnTime = TimeSpan.FromSeconds(60);

    // TODO: tirar coisas de leap daqui
    [DataField("leapDistance")]
    public float LeapDistance = 1f;

    [DataField("leapStrength")]
    public float LeapStrength = 4f;

    [DataField("feedingTime")]
    public TimeSpan FeedingTime = TimeSpan.FromSeconds(4);

    public EntityUid? Victim;

    public bool VictimResisted = false;

    [DataField("feedingQuantity")]
    public FixedPoint2 FeedingSolutionQuantity = FixedPoint2.New(5);

    [DataField("feedingSolution")]
    public string FeedingSolutionReagent = "Nutriment";

    [DataField("feedingDamage")]
    public DamageSpecifier FeedingDamage = default!;

    public ProtoId<EntityPrototype> SlimeNutrimentPrototype = "PuddleSlimeNutriment";
}

public sealed class SlimeTotallyFedEvent : EntityEventArgs
{
    public EntityUid Entity;

    public SlimeTotallyFedEvent(EntityUid entity)
    {
        Entity = entity;
    }
}

public sealed class LatchOnEvent : EntityEventArgs
{
    public EntityUid User;
    public EntityUid Target;

    public LatchOnEvent(EntityUid user, EntityUid target)
    {
        User = user;
        Target = target;
    }
}

public sealed class UnlatchOnEvent : EntityEventArgs
{
    public EntityUid User;
    public EntityUid Target;

    public UnlatchOnEvent(EntityUid user, EntityUid target)
    {
        User = user;
        Target = target;
    }
}

/// <summary>
///     Do after even for
/// </summary>
[Serializable, NetSerializable]
public sealed partial class FeedDoAfterEvent : DoAfterEvent
{
    public bool FeedCancelled = false;

    public FeedDoAfterEvent()
    {
    }
    public override DoAfterEvent Clone() => this;
}
