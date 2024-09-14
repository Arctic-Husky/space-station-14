<<<<<<< HEAD
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Nutrition.Components;
using Content.Shared.Overlays;
using Content.Shared.StatusIcon.Components;
=======
using Content.Shared.Nutrition.Components;
using Content.Shared.Overlays;
using Content.Shared.StatusIcon;
using Content.Shared.StatusIcon.Components;
using Robust.Shared.Prototypes;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

namespace Content.Client.Overlays;

public sealed class ShowThirstIconsSystem : EquipmentHudSystem<ShowThirstIconsComponent>
{
<<<<<<< HEAD
    [Dependency] private readonly ThirstSystem _thirst = default!;
=======
    [Dependency] private readonly IPrototypeManager _prototypeMan = default!;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ThirstComponent, GetStatusIconsEvent>(OnGetStatusIconsEvent);
    }

<<<<<<< HEAD
    private void OnGetStatusIconsEvent(EntityUid uid, ThirstComponent component, ref GetStatusIconsEvent ev)
    {
        if (!IsActive || ev.InContainer || ev.HasStealthComponent)
            return;

        if (_thirst.TryGetStatusIconPrototype(component, out var iconPrototype))
            ev.StatusIcons.Add(iconPrototype!);
=======
    private void OnGetStatusIconsEvent(EntityUid uid, ThirstComponent thirstComponent, ref GetStatusIconsEvent args)
    {
        if (!IsActive || args.InContainer)
            return;

        var thirstIcons = DecideThirstIcon(uid, thirstComponent);

        args.StatusIcons.AddRange(thirstIcons);
    }

    private IReadOnlyList<StatusIconPrototype> DecideThirstIcon(EntityUid uid, ThirstComponent thirstComponent)
    {
        var result = new List<StatusIconPrototype>();

        switch (thirstComponent.CurrentThirstThreshold)
        {
            case ThirstThreshold.OverHydrated:
                if (_prototypeMan.TryIndex<StatusIconPrototype>("ThirstIconOverhydrated", out var overhydrated))
                {
                    result.Add(overhydrated);
                }
                break;
            case ThirstThreshold.Thirsty:
                if (_prototypeMan.TryIndex<StatusIconPrototype>("ThirstIconThirsty", out var thirsty))
                {
                    result.Add(thirsty);
                }
                break;
            case ThirstThreshold.Parched:
                if (_prototypeMan.TryIndex<StatusIconPrototype>("ThirstIconParched", out var parched))
                {
                    result.Add(parched);
                }
                break;
        }

        return result;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }
}
