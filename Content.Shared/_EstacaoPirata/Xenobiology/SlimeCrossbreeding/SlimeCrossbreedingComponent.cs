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
    public List<SlimeExtractReactionEntry>? Crossbreeds;
}

// [DataDefinition]
// public sealed partial class SlimeCrossbreedingEntry
// {
//     [DataField("type", required: true)]
//     public string Type;
//
//     // Ter uma variavel List<string> que contera os reagentes
//
//     [DataField("effects", required: true)]
//     public List<SlimeReagentEffect> Effects = default!;
//
//     [DataField("sound")]
//     public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/Effects/Chemistry/bubbles.ogg",
//         new AudioParams
//         {
//             Volume = -10
//         });
// }
