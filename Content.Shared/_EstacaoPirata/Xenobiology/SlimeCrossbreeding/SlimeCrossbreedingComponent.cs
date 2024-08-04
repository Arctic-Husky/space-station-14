using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Robust.Shared.Audio;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeCrossbreeding;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class SlimeCrossbreedingComponent : Component
{
    [DataField("crossbreeds", true, serverOnly: true)]
    public List<SlimeCrossbreedingEntry>? Crossbreeds;
}

[DataDefinition]
public sealed partial class SlimeCrossbreedingEntry
{
    [DataField("prototype")]
    public string Prototype = default!;

    [DataField("reactions", true, serverOnly: true)]
    public List<SlimeExtractReactionEntry>? Reactions;
}
