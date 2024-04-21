namespace Content.Shared._EstacaoPirata.Xenobiology.Meiosis;

/// <summary>
/// This is used for controlling the process of meiosis of an entity.
/// </summary>
[RegisterComponent]
public sealed partial class MeiosisComponent : Component
{
    // Nao precisa disto, pode usar o Prototype no codigo pra obter o valor
    // /// <summary>
    // /// The string that represents this entity's prototype
    // /// </summary>
    // [DataField("prototype"), ViewVariables]
    // public string Prototype = string.Empty;

    /// <summary>
    /// A list of possible prototypes that can be produced via meiosis
    /// </summary>
    [DataField("neighbors"), ViewVariables]
    public HashSet<string> Neighbors = new();

    /// <summary>
    /// The chance this node has to mutate when meiosis occurs
    /// </summary>
    [DataField("mutationChance"), ViewVariables]
    public float MutationChance = 10f;

    // /// <summary>
    // /// Whether or not the node has been triggered
    // /// </summary>
    // [DataField("triggered"), ViewVariables(VVAccess.ReadWrite)]
    // public bool Triggered = false;

    /// <summary>
    /// Used for storing cumulative information about nodes
    /// </summary>
    [DataField("nodeData"), ViewVariables]
    public Dictionary<string, object> NodeData = new();
}
