using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared.Audio.Jukebox;

/// <summary>
/// Soundtrack that's visible on the jukebox list.
/// </summary>
[Prototype]
<<<<<<< HEAD
public sealed partial class JukeboxPrototype : IPrototype
=======
public sealed class JukeboxPrototype : IPrototype
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
{
    [IdDataField]
    public string ID { get; } = string.Empty;

    /// <summary>
    /// User friendly name to use in UI.
    /// </summary>
    [DataField(required: true)]
    public string Name = string.Empty;

    [DataField(required: true)]
    public SoundPathSpecifier Path = default!;
}
