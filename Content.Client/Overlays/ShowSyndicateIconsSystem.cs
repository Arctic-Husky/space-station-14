using Content.Shared.Overlays;
<<<<<<< HEAD
using Content.Shared.NukeOps;
using Content.Shared.StatusIcon;
using Content.Shared.StatusIcon.Components;
using Robust.Shared.Prototypes;

namespace Content.Client.Overlays;

=======
using Content.Shared.StatusIcon.Components;
using Content.Shared.NukeOps;
using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;

namespace Content.Client.Overlays;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
public sealed class ShowSyndicateIconsSystem : EquipmentHudSystem<ShowSyndicateIconsComponent>
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<NukeOperativeComponent, GetStatusIconsEvent>(OnGetStatusIconsEvent);
    }

<<<<<<< HEAD
    private void OnGetStatusIconsEvent(EntityUid uid, NukeOperativeComponent component, ref GetStatusIconsEvent ev)
    {
        if (!IsActive || ev.InContainer)
            return;

        if (_prototype.TryIndex<StatusIconPrototype>(component.SyndStatusIcon, out var iconPrototype))
            ev.StatusIcons.Add(iconPrototype);
=======
    private void OnGetStatusIconsEvent(EntityUid uid, NukeOperativeComponent nukeOperativeComponent, ref GetStatusIconsEvent args)
    {
        if (!IsActive || args.InContainer)
        {
            return;
        }

        var syndicateIcons = SyndicateIcon(uid, nukeOperativeComponent);

        args.StatusIcons.AddRange(syndicateIcons);
    }

    private IReadOnlyList<StatusIconPrototype> SyndicateIcon(EntityUid uid, NukeOperativeComponent nukeOperativeComponent)
    {
        var result = new List<StatusIconPrototype>();

        if (_prototype.TryIndex<StatusIconPrototype>(nukeOperativeComponent.SyndStatusIcon, out var syndicateicon))
        {
            result.Add(syndicateicon);
        }

        return result;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }
}

