using System.Linq;
using Content.Server.DoAfter;
using Content.Server.Humanoid;
using Content.Shared.UserInterface;
using Content.Shared.DoAfter;
using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Markings;
using Content.Shared.Interaction;
using Content.Shared.MagicMirror;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;

namespace Content.Server.MagicMirror;

/// <summary>
/// Allows humanoids to change their appearance mid-round.
/// </summary>
<<<<<<< HEAD
public sealed class MagicMirrorSystem : SharedMagicMirrorSystem
=======
public sealed class MagicMirrorSystem : EntitySystem
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly DoAfterSystem _doAfterSystem = default!;
    [Dependency] private readonly MarkingManager _markings = default!;
    [Dependency] private readonly HumanoidAppearanceSystem _humanoid = default!;
    [Dependency] private readonly SharedInteractionSystem _interaction = default!;
    [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MagicMirrorComponent, ActivatableUIOpenAttemptEvent>(OnOpenUIAttempt);

        Subs.BuiEvents<MagicMirrorComponent>(MagicMirrorUiKey.Key, subs =>
        {
<<<<<<< HEAD
            subs.Event<BoundUIClosedEvent>(OnUiClosed);
=======
            subs.Event<BoundUIClosedEvent>(OnUIClosed);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            subs.Event<MagicMirrorSelectMessage>(OnMagicMirrorSelect);
            subs.Event<MagicMirrorChangeColorMessage>(OnTryMagicMirrorChangeColor);
            subs.Event<MagicMirrorAddSlotMessage>(OnTryMagicMirrorAddSlot);
            subs.Event<MagicMirrorRemoveSlotMessage>(OnTryMagicMirrorRemoveSlot);
        });

        SubscribeLocalEvent<MagicMirrorComponent, AfterInteractEvent>(OnMagicMirrorInteract);

        SubscribeLocalEvent<MagicMirrorComponent, MagicMirrorSelectDoAfterEvent>(OnSelectSlotDoAfter);
        SubscribeLocalEvent<MagicMirrorComponent, MagicMirrorChangeColorDoAfterEvent>(OnChangeColorDoAfter);
        SubscribeLocalEvent<MagicMirrorComponent, MagicMirrorRemoveSlotDoAfterEvent>(OnRemoveSlotDoAfter);
        SubscribeLocalEvent<MagicMirrorComponent, MagicMirrorAddSlotDoAfterEvent>(OnAddSlotDoAfter);
<<<<<<< HEAD
=======

        SubscribeLocalEvent<MagicMirrorComponent, BoundUserInterfaceCheckRangeEvent>(OnMirrorRangeCheck);
    }

    private void OnMirrorRangeCheck(EntityUid uid, MagicMirrorComponent component, ref BoundUserInterfaceCheckRangeEvent args)
    {
        if (!Exists(component.Target) || !_interaction.InRangeUnobstructed(uid, component.Target.Value))
        {
            args.Result = BoundUserInterfaceRangeResult.Fail;
        }
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }

    private void OnMagicMirrorInteract(Entity<MagicMirrorComponent> mirror, ref AfterInteractEvent args)
    {
        if (!args.CanReach || args.Target == null)
            return;

<<<<<<< HEAD
        if (!_uiSystem.TryOpenUi(mirror.Owner, MagicMirrorUiKey.Key, args.User))
=======
        if (!TryComp<ActorComponent>(args.User, out var actor))
            return;

        if (!_uiSystem.TryOpen(mirror.Owner, MagicMirrorUiKey.Key, actor.PlayerSession))
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            return;

        UpdateInterface(mirror.Owner, args.Target.Value, mirror.Comp);
    }

    private void OnOpenUIAttempt(EntityUid uid, MagicMirrorComponent mirror, ActivatableUIOpenAttemptEvent args)
    {
        if (!HasComp<HumanoidAppearanceComponent>(args.User))
            args.Cancel();
    }

    private void OnMagicMirrorSelect(EntityUid uid, MagicMirrorComponent component, MagicMirrorSelectMessage message)
    {
<<<<<<< HEAD
        if (component.Target is not { } target)
=======
        if (component.Target is not { } target || message.Session.AttachedEntity is not { } user)
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            return;

        _doAfterSystem.Cancel(component.DoAfter);
        component.DoAfter = null;

        var doAfter = new MagicMirrorSelectDoAfterEvent()
        {
            Category = message.Category,
            Slot = message.Slot,
            Marking = message.Marking,
        };

<<<<<<< HEAD
        _doAfterSystem.TryStartDoAfter(new DoAfterArgs(EntityManager, message.Actor, component.SelectSlotTime, doAfter, uid, target: target, used: uid)
        {
            DistanceThreshold = SharedInteractionSystem.InteractionRange,
            BreakOnDamage = true,
            BreakOnMove = true,
            BreakOnHandChange = false,
=======
        _doAfterSystem.TryStartDoAfter(new DoAfterArgs(EntityManager, user, component.SelectSlotTime, doAfter, uid, target: target, used: uid)
        {
            DistanceThreshold = SharedInteractionSystem.InteractionRange,
            BreakOnTargetMove = true,
            BreakOnDamage = true,
            BreakOnHandChange = false,
            BreakOnUserMove = true,
            BreakOnWeightlessMove = false,
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            NeedHand = true
        }, out var doAfterId);

        component.DoAfter = doAfterId;
        _audio.PlayPvs(component.ChangeHairSound, uid);
    }

    private void OnSelectSlotDoAfter(EntityUid uid, MagicMirrorComponent component, MagicMirrorSelectDoAfterEvent args)
    {
        if (args.Handled || args.Target == null || args.Cancelled)
            return;

        if (component.Target != args.Target)
            return;

        MarkingCategories category;

        switch (args.Category)
        {
            case MagicMirrorCategory.Hair:
                category = MarkingCategories.Hair;
                break;
            case MagicMirrorCategory.FacialHair:
                category = MarkingCategories.FacialHair;
                break;
            default:
                return;
        }

        _humanoid.SetMarkingId(component.Target.Value, category, args.Slot, args.Marking);

        UpdateInterface(uid, component.Target.Value, component);
    }

    private void OnTryMagicMirrorChangeColor(EntityUid uid, MagicMirrorComponent component, MagicMirrorChangeColorMessage message)
    {
<<<<<<< HEAD
        if (component.Target is not { } target)
=======
        if (component.Target is not { } target || message.Session.AttachedEntity is not { } user)
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            return;

        _doAfterSystem.Cancel(component.DoAfter);
        component.DoAfter = null;

        var doAfter = new MagicMirrorChangeColorDoAfterEvent()
        {
            Category = message.Category,
            Slot = message.Slot,
            Colors = message.Colors,
        };

<<<<<<< HEAD
        _doAfterSystem.TryStartDoAfter(new DoAfterArgs(EntityManager, message.Actor, component.ChangeSlotTime, doAfter, uid, target: target, used: uid)
        {
            BreakOnDamage = true,
            BreakOnMove = true,
            BreakOnHandChange = false,
=======
        _doAfterSystem.TryStartDoAfter(new DoAfterArgs(EntityManager, user, component.ChangeSlotTime, doAfter, uid, target: target, used: uid)
        {
            BreakOnTargetMove = true,
            BreakOnDamage = true,
            BreakOnHandChange = false,
            BreakOnUserMove = true,
            BreakOnWeightlessMove = false,
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            NeedHand = true
        }, out var doAfterId);

        component.DoAfter = doAfterId;
    }
    private void OnChangeColorDoAfter(EntityUid uid, MagicMirrorComponent component, MagicMirrorChangeColorDoAfterEvent args)
    {
        if (args.Handled || args.Target == null || args.Cancelled)
            return;

        if (component.Target != args.Target)
            return;

        MarkingCategories category;
        switch (args.Category)
        {
            case MagicMirrorCategory.Hair:
                category = MarkingCategories.Hair;
                break;
            case MagicMirrorCategory.FacialHair:
                category = MarkingCategories.FacialHair;
                break;
            default:
                return;
        }

        _humanoid.SetMarkingColor(component.Target.Value, category, args.Slot, args.Colors);

        // using this makes the UI feel like total ass
        // que
        // UpdateInterface(uid, component.Target, message.Session);
    }

    private void OnTryMagicMirrorRemoveSlot(EntityUid uid, MagicMirrorComponent component, MagicMirrorRemoveSlotMessage message)
    {
<<<<<<< HEAD
        if (component.Target is not { } target)
=======
        if (component.Target is not { } target || message.Session.AttachedEntity is not { } user)
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            return;

        _doAfterSystem.Cancel(component.DoAfter);
        component.DoAfter = null;

        var doAfter = new MagicMirrorRemoveSlotDoAfterEvent()
        {
            Category = message.Category,
            Slot = message.Slot,
        };

<<<<<<< HEAD
        _doAfterSystem.TryStartDoAfter(new DoAfterArgs(EntityManager, message.Actor, component.RemoveSlotTime, doAfter, uid, target: target, used: uid)
        {
            DistanceThreshold = SharedInteractionSystem.InteractionRange,
            BreakOnDamage = true,
            BreakOnHandChange = false,
=======
        _doAfterSystem.TryStartDoAfter(new DoAfterArgs(EntityManager, user, component.RemoveSlotTime, doAfter, uid, target: target, used: uid)
        {
            DistanceThreshold = SharedInteractionSystem.InteractionRange,
            BreakOnTargetMove = true,
            BreakOnDamage = true,
            BreakOnHandChange = false,
            BreakOnUserMove = true,
            BreakOnWeightlessMove = false,
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            NeedHand = true
        }, out var doAfterId);

        component.DoAfter = doAfterId;
        _audio.PlayPvs(component.ChangeHairSound, uid);
    }

    private void OnRemoveSlotDoAfter(EntityUid uid, MagicMirrorComponent component, MagicMirrorRemoveSlotDoAfterEvent args)
    {
        if (args.Handled || args.Target == null || args.Cancelled)
            return;

        if (component.Target != args.Target)
            return;

        MarkingCategories category;

        switch (args.Category)
        {
            case MagicMirrorCategory.Hair:
                category = MarkingCategories.Hair;
                break;
            case MagicMirrorCategory.FacialHair:
                category = MarkingCategories.FacialHair;
                break;
            default:
                return;
        }

        _humanoid.RemoveMarking(component.Target.Value, category, args.Slot);

        UpdateInterface(uid, component.Target.Value, component);
    }

    private void OnTryMagicMirrorAddSlot(EntityUid uid, MagicMirrorComponent component, MagicMirrorAddSlotMessage message)
    {
        if (component.Target == null)
            return;

<<<<<<< HEAD
=======
        if (message.Session.AttachedEntity == null)
            return;

>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        _doAfterSystem.Cancel(component.DoAfter);
        component.DoAfter = null;

        var doAfter = new MagicMirrorAddSlotDoAfterEvent()
        {
            Category = message.Category,
        };

<<<<<<< HEAD
        _doAfterSystem.TryStartDoAfter(new DoAfterArgs(EntityManager, message.Actor, component.AddSlotTime, doAfter, uid, target: component.Target.Value, used: uid)
        {
            BreakOnDamage = true,
            BreakOnMove = true,
            BreakOnHandChange = false,
=======
        _doAfterSystem.TryStartDoAfter(new DoAfterArgs(EntityManager, message.Session.AttachedEntity.Value, component.AddSlotTime, doAfter, uid, target: component.Target.Value, used: uid)
        {
            BreakOnTargetMove = true,
            BreakOnDamage = true,
            BreakOnHandChange = false,
            BreakOnUserMove = true,
            BreakOnWeightlessMove = false,
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            NeedHand = true
        }, out var doAfterId);

        component.DoAfter = doAfterId;
        _audio.PlayPvs(component.ChangeHairSound, uid);
    }
    private void OnAddSlotDoAfter(EntityUid uid, MagicMirrorComponent component, MagicMirrorAddSlotDoAfterEvent args)
    {
        if (args.Handled || args.Target == null || args.Cancelled || !TryComp(component.Target, out HumanoidAppearanceComponent? humanoid))
            return;

        MarkingCategories category;

        switch (args.Category)
        {
            case MagicMirrorCategory.Hair:
                category = MarkingCategories.Hair;
                break;
            case MagicMirrorCategory.FacialHair:
                category = MarkingCategories.FacialHair;
                break;
            default:
                return;
        }

        var marking = _markings.MarkingsByCategoryAndSpecies(category, humanoid.Species).Keys.FirstOrDefault();

        if (string.IsNullOrEmpty(marking))
            return;

        _humanoid.AddMarking(component.Target.Value, marking, Color.Black);

        UpdateInterface(uid, component.Target.Value, component);

    }

    private void UpdateInterface(EntityUid mirrorUid, EntityUid targetUid, MagicMirrorComponent component)
    {
        if (!TryComp<HumanoidAppearanceComponent>(targetUid, out var humanoid))
            return;

        var hair = humanoid.MarkingSet.TryGetCategory(MarkingCategories.Hair, out var hairMarkings)
            ? new List<Marking>(hairMarkings)
            : new();

        var facialHair = humanoid.MarkingSet.TryGetCategory(MarkingCategories.FacialHair, out var facialHairMarkings)
            ? new List<Marking>(facialHairMarkings)
            : new();

        var state = new MagicMirrorUiState(
            humanoid.Species,
            hair,
            humanoid.MarkingSet.PointsLeft(MarkingCategories.Hair) + hair.Count,
            facialHair,
            humanoid.MarkingSet.PointsLeft(MarkingCategories.FacialHair) + facialHair.Count);

<<<<<<< HEAD
        // TODO: Component states
        component.Target = targetUid;
        _uiSystem.SetUiState(mirrorUid, MagicMirrorUiKey.Key, state);
        Dirty(mirrorUid, component);
    }

    private void OnUiClosed(Entity<MagicMirrorComponent> ent, ref BoundUIClosedEvent args)
    {
        ent.Comp.Target = null;
        Dirty(ent);
=======
        component.Target = targetUid;
        _uiSystem.TrySetUiState(mirrorUid, MagicMirrorUiKey.Key, state);
    }

    private void OnUIClosed(Entity<MagicMirrorComponent> ent, ref BoundUIClosedEvent args)
    {
        ent.Comp.Target = null;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }
}
