using Content.Server.Administration.Managers;
using Content.Shared.Administration;
using Content.Shared.Humanoid;
using Content.Shared.Verbs;
using Robust.Server.GameObjects;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Server.Humanoid;

public sealed partial class HumanoidAppearanceSystem
{
    [Dependency] private readonly IAdminManager _adminManager = default!;
    [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;

    private void OnVerbsRequest(EntityUid uid, HumanoidAppearanceComponent component, GetVerbsEvent<Verb> args)
    {
        if (!TryComp<ActorComponent>(args.User, out var actor))
        {
            return;
        }

        if (!_adminManager.HasAdminFlag(actor.PlayerSession, AdminFlags.Fun))
        {
            return;
        }

        args.Verbs.Add(new Verb
        {
            Text = "Modify markings",
            Category = VerbCategory.Tricks,
            Icon = new SpriteSpecifier.Rsi(new("/Textures/Mobs/Customization/reptilian_parts.rsi"), "tail_smooth"),
            Act = () =>
            {
                _uiSystem.OpenUi(uid, HumanoidMarkingModifierKey.Key, actor.PlayerSession);
                _uiSystem.SetUiState(
                    uid,
                    HumanoidMarkingModifierKey.Key,
                    new HumanoidMarkingModifierState(component.MarkingSet, component.Species,
                        component.Sex,
                        component.SkinColor,
                        component.CustomBaseLayers
                    ));
            }
        });
    }

    private void OnBaseLayersSet(EntityUid uid, HumanoidAppearanceComponent component,
        HumanoidMarkingModifierBaseLayersSetMessage message)
    {
<<<<<<< HEAD
        if (!_adminManager.HasAdminFlag(message.Actor, AdminFlags.Fun))
=======
        if (message.Session is not { } player
            || !_adminManager.HasAdminFlag(player, AdminFlags.Fun))
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        {
            return;
        }

        if (message.Info == null)
        {
            component.CustomBaseLayers.Remove(message.Layer);
        }
        else
        {
            component.CustomBaseLayers[message.Layer] = message.Info.Value;
        }

        Dirty(uid, component);

        if (message.ResendState)
        {
            _uiSystem.SetUiState(
                uid,
                HumanoidMarkingModifierKey.Key,
                new HumanoidMarkingModifierState(component.MarkingSet, component.Species,
                        component.Sex,
                        component.SkinColor,
                        component.CustomBaseLayers
                    ));
        }
    }

    private void OnMarkingsSet(EntityUid uid, HumanoidAppearanceComponent component,
        HumanoidMarkingModifierMarkingSetMessage message)
    {
<<<<<<< HEAD
        if (!_adminManager.HasAdminFlag(message.Actor, AdminFlags.Fun))
=======
        if (message.Session is not { } player
            || !_adminManager.HasAdminFlag(player, AdminFlags.Fun))
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        {
            return;
        }

        component.MarkingSet = message.MarkingSet;
        Dirty(uid, component);

        if (message.ResendState)
        {
            _uiSystem.SetUiState(
                uid,
                HumanoidMarkingModifierKey.Key,
                new HumanoidMarkingModifierState(component.MarkingSet, component.Species,
                        component.Sex,
                        component.SkinColor,
                        component.CustomBaseLayers
                    ));
        }

    }
}
