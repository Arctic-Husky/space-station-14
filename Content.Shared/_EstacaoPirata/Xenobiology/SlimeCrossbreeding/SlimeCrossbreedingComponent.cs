using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Robust.Shared.Audio;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeCrossbreeding;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class SlimeCrossbreedingComponent : Component
{
    [DataField("crossbreeds", true, serverOnly: true)] // Transformar isso em dictionary pra nao precisar converter depois
    public Dictionary<string, SlimeCrossbreedingEntry>? Crossbreeds;
    // public List<SlimeCrossbreedingEntry>? Crossbreeds;

    [ViewVariables]
    public Dictionary<string, int> ExtractsUsed = new();

    [ViewVariables]
    public bool MaxAchieved = false;

    public string MaxColor = default!;

    [ViewVariables]
    public int Max = 10;
}

[DataDefinition]
public sealed partial class SlimeCrossbreedingEntry
{
    [DataField("prototype")]
    public string Prototype = default!;

    // [DataField("color")]
    // public string Color = default!;

    [DataField("reactions", true, serverOnly: true)]
    public List<SlimeExtractReactionEntry> Reactions;
}
