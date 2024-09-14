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

public sealed class ShowHungerIconsSystem : EquipmentHudSystem<ShowHungerIconsComponent>
{
<<<<<<< HEAD
    [Dependency] private readonly HungerSystem _hunger = default!;
=======
    [Dependency] private readonly IPrototypeManager _prototypeMan = default!;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HungerComponent, GetStatusIconsEvent>(OnGetStatusIconsEvent);
    }

<<<<<<< HEAD
    private void OnGetStatusIconsEvent(EntityUid uid, HungerComponent component, ref GetStatusIconsEvent ev)
    {
        if (!IsActive || ev.InContainer || ev.HasStealthComponent)
            return;

        if (_hunger.TryGetStatusIconPrototype(component, out var iconPrototype))
            ev.StatusIcons.Add(iconPrototype);
=======
    private void OnGetStatusIconsEvent(EntityUid uid, HungerComponent hungerComponent, ref GetStatusIconsEvent args)
    {
        if (!IsActive || args.InContainer)
            return;

        var hungerIcons = DecideHungerIcon(uid, hungerComponent);

        args.StatusIcons.AddRange(hungerIcons);
    }

    private IReadOnlyList<StatusIconPrototype> DecideHungerIcon(EntityUid uid, HungerComponent hungerComponent)
    {
        var result = new List<StatusIconPrototype>();

        switch (hungerComponent.CurrentThreshold)
        {
            case HungerThreshold.Overfed:
                if (_prototypeMan.TryIndex<StatusIconPrototype>("HungerIconOverfed", out var overfed))
                {
                    result.Add(overfed);
                }
                break;
            case HungerThreshold.Peckish:
                if (_prototypeMan.TryIndex<StatusIconPrototype>("HungerIconPeckish", out var peckish))
                {
                    result.Add(peckish);
                }
                break;
            case HungerThreshold.Starving:
                if (_prototypeMan.TryIndex<StatusIconPrototype>("HungerIconStarving", out var starving))
                {
                    result.Add(starving);
                }
                break;
        }

        return result;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }
}
