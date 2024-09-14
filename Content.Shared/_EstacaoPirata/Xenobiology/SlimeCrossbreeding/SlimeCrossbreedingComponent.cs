using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Robust.Shared.Audio;
using Robust.Shared.Serialization;

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

    [DataField("sound")]
    public SoundSpecifier Sound = new SoundCollectionSpecifier("SlimeGore");

    [DataField("soundComplete")]
    public SoundSpecifier SoundComplete = new SoundPathSpecifier("/Audio/Effects/Chemistry/bubbles.ogg", new AudioParams{Volume = -2});
}

[DataDefinition]
public sealed partial class SlimeCrossbreedingEntry
{
    [DataField("prototype")]
    public string Prototype = default!;

    [DataField("color")]
    public Color Color = Color.Transparent;

    [DataField("reactions", true, serverOnly: true)]
    public List<SlimeExtractReactionEntry> Reactions;
}

[Serializable, NetSerializable]
public sealed class ExtractColorChangeEvent(NetEntity extract, Color color) : EntityEventArgs
{
    public NetEntity Extract = extract;
    public Color Color = color;
}
