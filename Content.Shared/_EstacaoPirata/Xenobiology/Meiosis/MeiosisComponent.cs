using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
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
    [DataField("mutations"), ViewVariables]
    public HashSet<ProtoId<EntityPrototype>> Mutations = new();

    /// <summary>
    /// The chance this node has to mutate when meiosis occurs, must be a value between 0 and 1
    /// </summary>
    [DataField("mutationChance"), ViewVariables]
    public MeiosisThreshold MutationChance = MeiosisThreshold.Mid;

    /// <summary>
    /// A dictionary relating MeiosisThreshold to how much they modify
    /// </summary>
    [DataField("mutationSeverities")]
    public Dictionary<MeiosisThreshold, (float, float)> MutationSeverities = new()
    {
        { MeiosisThreshold.Max , (1f, 1f)},
        { MeiosisThreshold.Severe, (0.8f, 0.99f) },
        { MeiosisThreshold.High, (0.6f, 0.79f) },
        { MeiosisThreshold.Mid, (0.25f, 0.45f) },
        { MeiosisThreshold.Low, (0.15f, 0.24f) }
    };

    /// <summary>
    /// Number of babies that will be made when meiosis occurs
    /// </summary>
    [DataField("numberOfBabies"), ViewVariables]
    public int NumberOfBabies = 4;

    /// <summary>
    /// Used for storing cumulative information about nodes
    /// </summary>
    [DataField("nodeData"), ViewVariables]
    public Dictionary<string, object> NodeData = new();

    [ViewVariables]
    public FixedPoint2 AccumulatedMutagen = FixedPoint2.Zero;

    [DataField("sound")]
    public SoundSpecifier? MeiosisSound = new SoundPathSpecifier("/Audio/EstacaoPirata/Effects/Slime/slime_jump.ogg");
}

public enum MeiosisThreshold : byte
{
    Low,
    Mid,
    High,
    Severe,
    Max
}
