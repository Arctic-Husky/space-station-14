using System.Text.RegularExpressions;
using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class SouthernAccentSystem : EntitySystem
{
<<<<<<< HEAD
    private static readonly Regex RegexIng = new(@"ing\b");
    private static readonly Regex RegexAnd = new(@"\band\b");
    private static readonly Regex RegexDve = new("d've");

    [Dependency] private readonly ReplacementAccentSystem _replacement = default!;

=======
    [Dependency] private readonly ReplacementAccentSystem _replacement = default!;
    
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SouthernAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, SouthernAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;

        message = _replacement.ApplyReplacements(message, "southern");

        //They shoulda started runnin' an' hidin' from me!
<<<<<<< HEAD
        message = RegexIng.Replace(message, "in'");
        message = RegexAnd.Replace(message, "an'");
        message = RegexDve.Replace(message, "da");
=======
        message = Regex.Replace(message, @"ing\b", "in'");
        message = Regex.Replace(message, @"\band\b", "an'");
        message = Regex.Replace(message, "d've", "da");
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        args.Message = message;
    }
};
