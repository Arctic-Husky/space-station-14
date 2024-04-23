using Robust.Shared.GameStates;
using Robust.Shared.Map;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeFeeding;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SlimeLeapingComponent : Component
{
    [DataField, AutoNetworkedField]
    public EntityCoordinates Origin;

    [DataField, AutoNetworkedField]
    public TimeSpan LeapEndTime;
}
