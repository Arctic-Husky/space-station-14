using Content.Server.Chat;
<<<<<<< HEAD
=======
using Content.Server.Announcements.Systems;
using Robust.Shared.Player;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

namespace Content.Server.Chat.Systems;

public sealed class AnnounceOnSpawnSystem : EntitySystem
{
    [Dependency] private readonly ChatSystem _chat = default!;
<<<<<<< HEAD
=======
    [Dependency] private readonly AnnouncerSystem _announcer = default!;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AnnounceOnSpawnComponent, MapInitEvent>(OnInit);
    }

    private void OnInit(EntityUid uid, AnnounceOnSpawnComponent comp, MapInitEvent args)
    {
<<<<<<< HEAD
        var message = Loc.GetString(comp.Message);
        var sender = comp.Sender != null ? Loc.GetString(comp.Sender) : "Central Command";
        _chat.DispatchGlobalAnnouncement(message, sender, playSound: true, comp.Sound, comp.Color);
=======
        var sender = comp.Sender != null ? Loc.GetString(comp.Sender) : "Central Command";
        _announcer.SendAnnouncement(_announcer.GetAnnouncementId("SpawnAnnounceCaptain"), Filter.Broadcast(),
            comp.Message, sender, comp.Color);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }
}
