using Robust.Shared.Player;

namespace Content.Server.Arcade.BlockGame;

[RegisterComponent]
public sealed partial class BlockGameArcadeComponent : Component
{
    /// <summary>
    /// The currently active session of NT-BG.
    /// </summary>
    public BlockGame? Game = null;

    /// <summary>
    /// The player currently playing the active session of NT-BG.
    /// </summary>
<<<<<<< HEAD
    public EntityUid? Player = null;
=======
    public ICommonSession? Player = null;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    /// <summary>
    /// The players currently viewing (but not playing) the active session of NT-BG.
    /// </summary>
<<<<<<< HEAD
    public readonly List<EntityUid> Spectators = new();
=======
    public readonly List<ICommonSession> Spectators = new();
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
}
