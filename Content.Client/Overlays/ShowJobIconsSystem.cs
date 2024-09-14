using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.Mindshield.Components;
using Content.Shared.Overlays;
using Content.Shared.PDA;
using Content.Shared.Security.Components;
using Content.Shared.StatusIcon;
using Content.Shared.StatusIcon.Components;
using Robust.Shared.Prototypes;

namespace Content.Client.Overlays;

<<<<<<< HEAD:Content.Client/Overlays/ShowJobIconsSystem.cs
public sealed class ShowJobIconsSystem : EquipmentHudSystem<ShowJobIconsComponent>
=======
public sealed class ShowSecurityIconsSystem : EquipmentHudSystem<ShowSecurityIconsComponent>
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f:Content.Client/Overlays/ShowSecurityIconsSystem.cs
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly AccessReaderSystem _accessReader = default!;

    [ValidatePrototypeId<StatusIconPrototype>]
    private const string JobIconForNoId = "JobIconNoId";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<StatusIconComponent, GetStatusIconsEvent>(OnGetStatusIconsEvent);
    }

    private void OnGetStatusIconsEvent(EntityUid uid, StatusIconComponent _, ref GetStatusIconsEvent ev)
    {
        if (!IsActive || ev.InContainer)
            return;

<<<<<<< HEAD:Content.Client/Overlays/ShowJobIconsSystem.cs
        var iconId = JobIconForNoId;

=======
        var securityIcons = DecideSecurityIcon(uid);

        @event.StatusIcons.AddRange(securityIcons);
    }

    private IReadOnlyList<StatusIconPrototype> DecideSecurityIcon(EntityUid uid)
    {
        var result = new List<StatusIconPrototype>();

        var jobIconToGet = JobIconForNoId;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f:Content.Client/Overlays/ShowSecurityIconsSystem.cs
        if (_accessReader.FindAccessItemsInventory(uid, out var items))
        {
            foreach (var item in items)
            {
                // ID Card
                if (TryComp<IdCardComponent>(item, out var id))
                {
                    iconId = id.JobIcon;
                    break;
                }

                // PDA
                if (TryComp<PdaComponent>(item, out var pda)
                    && pda.ContainedId != null
                    && TryComp(pda.ContainedId, out id))
                {
                    iconId = id.JobIcon;
                    break;
                }
            }
        }

        if (_prototype.TryIndex<StatusIconPrototype>(iconId, out var iconPrototype))
            ev.StatusIcons.Add(iconPrototype);
        else
<<<<<<< HEAD:Content.Client/Overlays/ShowJobIconsSystem.cs
            Log.Error($"Invalid job icon prototype: {iconPrototype}");
=======
            Log.Error($"Invalid job icon prototype: {jobIcon}");

        if (TryComp<MindShieldComponent>(uid, out var comp))
        {
            if (_prototypeMan.TryIndex<StatusIconPrototype>(comp.MindShieldStatusIcon.Id, out var icon))
                result.Add(icon);
        }

        if (TryComp<CriminalRecordComponent>(uid, out var record))
        {
            if(_prototypeMan.TryIndex<StatusIconPrototype>(record.StatusIcon.Id, out var criminalIcon))
                result.Add(criminalIcon);
        }

        return result;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f:Content.Client/Overlays/ShowSecurityIconsSystem.cs
    }
}
