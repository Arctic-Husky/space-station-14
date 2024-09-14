using Content.Server.Administration.Logs;
using Content.Server.Chat.Systems;
using Content.Server.Popups;
using Content.Shared.Clothing;
using Content.Shared.Database;
<<<<<<< HEAD
=======
using Content.Shared.Inventory.Events;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
using Content.Shared.Popups;
using Content.Shared.Preferences;
using Content.Shared.Speech;
using Content.Shared.VoiceMask;
using Robust.Server.GameObjects;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.VoiceMask;

public sealed partial class VoiceMaskSystem : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<VoiceMaskComponent, TransformSpeakerNameEvent>(OnSpeakerNameTransform);
        SubscribeLocalEvent<VoiceMaskComponent, VoiceMaskChangeNameMessage>(OnChangeName);
        SubscribeLocalEvent<VoiceMaskComponent, VoiceMaskChangeVerbMessage>(OnChangeVerb);
        SubscribeLocalEvent<VoiceMaskComponent, WearerMaskToggledEvent>(OnMaskToggled);
<<<<<<< HEAD
        SubscribeLocalEvent<VoiceMaskerComponent, ClothingGotEquippedEvent>(OnEquip);
        SubscribeLocalEvent<VoiceMaskerComponent, ClothingGotUnequippedEvent>(OnUnequip);
=======
        SubscribeLocalEvent<VoiceMaskerComponent, GotEquippedEvent>(OnEquip);
        SubscribeLocalEvent<VoiceMaskerComponent, GotUnequippedEvent>(OnUnequip);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        SubscribeLocalEvent<VoiceMaskSetNameEvent>(OnSetName);
        // SubscribeLocalEvent<VoiceMaskerComponent, GetVerbsEvent<AlternativeVerb>>(GetVerbs);
    }

    private void OnSetName(VoiceMaskSetNameEvent ev)
    {
        OpenUI(ev.Performer);
    }

    private void OnChangeName(EntityUid uid, VoiceMaskComponent component, VoiceMaskChangeNameMessage message)
    {
        if (message.Name.Length > HumanoidCharacterProfile.MaxNameLength || message.Name.Length <= 0)
        {
<<<<<<< HEAD
            _popupSystem.PopupEntity(Loc.GetString("voice-mask-popup-failure"), uid, message.Actor, PopupType.SmallCaution);
=======
            _popupSystem.PopupEntity(Loc.GetString("voice-mask-popup-failure"), uid, message.Session, PopupType.SmallCaution);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            return;
        }

        component.VoiceName = message.Name;
        _adminLogger.Add(LogType.Action, LogImpact.Medium, $"{ToPrettyString(message.Actor):player} set voice of {ToPrettyString(uid):mask}: {component.VoiceName}");

<<<<<<< HEAD
        _popupSystem.PopupEntity(Loc.GetString("voice-mask-popup-success"), uid, message.Actor);
=======
        _popupSystem.PopupEntity(Loc.GetString("voice-mask-popup-success"), uid, message.Session);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

        TrySetLastKnownName(uid, message.Name);

        UpdateUI(uid, component);
    }

    private void OnChangeVerb(Entity<VoiceMaskComponent> ent, ref VoiceMaskChangeVerbMessage msg)
    {
        if (msg.Verb is {} id && !_proto.HasIndex<SpeechVerbPrototype>(id))
            return;

        ent.Comp.SpeechVerb = msg.Verb;
        // verb is only important to metagamers so no need to log as opposed to name

<<<<<<< HEAD
        _popupSystem.PopupEntity(Loc.GetString("voice-mask-popup-success"), ent, msg.Actor);
=======
        _popupSystem.PopupEntity(Loc.GetString("voice-mask-popup-success"), ent, msg.Session);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

        TrySetLastSpeechVerb(ent, msg.Verb);

        UpdateUI(ent, ent.Comp);
    }

    private void OnSpeakerNameTransform(EntityUid uid, VoiceMaskComponent component, TransformSpeakerNameEvent args)
    {
        if (component.Enabled)
        {
            /*
            args.Name = _idCard.TryGetIdCard(uid, out var card) && !string.IsNullOrEmpty(card.FullName)
                ? card.FullName
                : Loc.GetString("voice-mask-unknown");
                */

            args.Name = component.VoiceName;
            if (component.SpeechVerb != null)
                args.SpeechVerb = component.SpeechVerb;
        }
    }

    private void OnMaskToggled(Entity<VoiceMaskComponent> ent, ref WearerMaskToggledEvent args)
<<<<<<< HEAD
    {
        ent.Comp.Enabled = !args.IsToggled;
    }

    private void OpenUI(EntityUid player)
    {
        if (!_uiSystem.HasUi(player, VoiceMaskUIKey.Key))
=======
    {
        ent.Comp.Enabled = !args.IsToggled;
    }

    private void OpenUI(EntityUid player, ActorComponent? actor = null)
    {
        // Delta-V: `logMissing: false` because of syrinx.
        if (!Resolve(player, ref actor, logMissing: false))
            return;
        if (!_uiSystem.TryGetUi(player, VoiceMaskUIKey.Key, out var bui))
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            return;

        _uiSystem.OpenUi(player, VoiceMaskUIKey.Key, player);
        UpdateUI(player);
    }

    private void UpdateUI(EntityUid owner, VoiceMaskComponent? component = null)
    {
        // Delta-V: `logMissing: false` because of syrinx
        if (!Resolve(owner, ref component, logMissing: false))
        {
            return;
        }

<<<<<<< HEAD
        if (_uiSystem.HasUi(owner, VoiceMaskUIKey.Key))
            _uiSystem.SetUiState(owner, VoiceMaskUIKey.Key, new VoiceMaskBuiState(component.VoiceName, component.SpeechVerb));
=======
        if (_uiSystem.TryGetUi(owner, VoiceMaskUIKey.Key, out var bui))
            _uiSystem.SetUiState(bui, new VoiceMaskBuiState(component.VoiceName, component.SpeechVerb));
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }
}
