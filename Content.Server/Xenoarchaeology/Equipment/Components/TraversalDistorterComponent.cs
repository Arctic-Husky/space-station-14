namespace Content.Server.Xenoarchaeology.Equipment.Components;

/// <summary>
/// This is used for a machine that biases
/// an artifact placed on it to move up/down
/// </summary>
[RegisterComponent]
public sealed partial class TraversalDistorterComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
<<<<<<< HEAD
    public BiasDirection BiasDirection = BiasDirection.Up;
=======
    public BiasDirection BiasDirection = BiasDirection.In;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    public TimeSpan NextActivation = default!;
    public TimeSpan ActivationDelay = TimeSpan.FromSeconds(1);
}

public enum BiasDirection : byte
{
    Up, //Towards depth 0
    Down, //Away from depth 0
}
