using Content.Shared.Chat;
using Content.Shared.Language;
using Content.Shared.Radio;

namespace Content.Server.Radio;

/// <summary>
/// <param name="OriginalChatMsg">The message to display when the speaker can understand "language"</param>
/// <param name="LanguageObfuscatedChatMsg">The message to display when the speaker cannot understand "language"</param>
/// </summary>
[ByRefEvent]
<<<<<<< HEAD
public readonly record struct RadioReceiveEvent(string Message, EntityUid MessageSource, RadioChannelPrototype Channel, EntityUid RadioSource, MsgChatMessage ChatMsg);
=======
public readonly record struct RadioReceiveEvent(
    // Einstein-Engines - languages mechanic
    EntityUid MessageSource,
    RadioChannelPrototype Channel,
    ChatMessage OriginalChatMsg,
    ChatMessage LanguageObfuscatedChatMsg,
    LanguagePrototype Language
);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

/// <summary>
/// Use this event to cancel sending message per receiver
/// </summary>
[ByRefEvent]
public record struct RadioReceiveAttemptEvent(RadioChannelPrototype Channel, EntityUid RadioSource, EntityUid RadioReceiver)
{
    public readonly RadioChannelPrototype Channel = Channel;
    public readonly EntityUid RadioSource = RadioSource;
    public readonly EntityUid RadioReceiver = RadioReceiver;
    public bool Cancelled = false;
}

/// <summary>
/// Use this event to cancel sending message to every receiver
/// </summary>
[ByRefEvent]
public record struct RadioSendAttemptEvent(RadioChannelPrototype Channel, EntityUid RadioSource)
{
    public readonly RadioChannelPrototype Channel = Channel;
    public readonly EntityUid RadioSource = RadioSource;
    public bool Cancelled = false;
}
