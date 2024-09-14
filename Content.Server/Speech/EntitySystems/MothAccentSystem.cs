using System.Text.RegularExpressions;
using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class MothAccentSystem : EntitySystem
{
<<<<<<< HEAD
    private static readonly Regex RegexLowerBuzz = new Regex("z{1,3}");
    private static readonly Regex RegexUpperBuzz = new Regex("Z{1,3}");

=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MothAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, MothAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;

        // buzzz
<<<<<<< HEAD
        message = RegexLowerBuzz.Replace(message, "zzz");
        // buZZZ
        message = RegexUpperBuzz.Replace(message, "ZZZ");

=======
        message = Regex.Replace(message, "z{1,3}", "zzz");
        // buZZZ
        message = Regex.Replace(message, "Z{1,3}", "ZZZ");
        
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        args.Message = message;
    }
}
