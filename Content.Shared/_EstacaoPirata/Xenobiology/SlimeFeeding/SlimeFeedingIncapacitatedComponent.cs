using Robust.Shared.GameStates;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeFeeding;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SlimeFeedingIncapacitatedComponent : Component
{
    [DataField, AutoNetworkedField]
    public TimeSpan RecoverAt;

    // Testar isto aqui
    [AutoNetworkedField]
    public EntityUid Attacker;
}
