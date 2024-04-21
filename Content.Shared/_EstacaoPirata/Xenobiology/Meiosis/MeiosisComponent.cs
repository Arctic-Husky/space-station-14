using Robust.Shared.Prototypes;

namespace Content.Shared._EstacaoPirata.Xenobiology.Meiosis;

/// <summary>
/// This is used for controlling the process of meiosis of an entity.
/// </summary>
[RegisterComponent]
public sealed partial class MeiosisComponent : Component
{
    /// <summary>
    /// This entity's baby version
    /// </summary>
    [DataField("baby"), ViewVariables]
    public ProtoId<EntityPrototype> Baby;

    /// <summary>
    /// A list of possible prototypes that can be produced via meiosis
    /// </summary>
    [DataField("neighbors"), ViewVariables]
    public HashSet<ProtoId<EntityPrototype>> Neighbors = new();

    /// <summary>
    /// The chance this node has to mutate when meiosis occurs, must be a value between 0 and 1
    /// </summary>
    [DataField("mutationChance"), ViewVariables]
    public float MutationChance = 0.1f;

    /// <summary>
    /// Number of babies that will be made when meiosis occurs
    /// </summary>
    [DataField("numberOfBabies"), ViewVariables]
    public int NumberOfBabies = 4;

    #region Hunger

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

    #endregion

    /// <summary>
    /// Used for storing cumulative information about nodes
    /// </summary>
    [DataField("nodeData"), ViewVariables]
    public Dictionary<string, object> NodeData = new();
}
