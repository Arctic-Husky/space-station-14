using Content.Server.Speech.Components;
using System.Text.RegularExpressions;

namespace Content.Server.Speech.EntitySystems;

/// <summary>
/// System that gives the speaker a faux-French accent.
/// </summary>
public sealed class FrenchAccentSystem : EntitySystem
{
    [Dependency] private readonly ReplacementAccentSystem _replacement = default!;

<<<<<<< HEAD
    private static readonly Regex RegexTh = new(@"th", RegexOptions.IgnoreCase);
    private static readonly Regex RegexStartH = new(@"(?<!\w)h", RegexOptions.IgnoreCase);
    private static readonly Regex RegexSpacePunctuation = new(@"(?<=\w\w)[!?;:](?!\w)", RegexOptions.IgnoreCase);

=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FrenchAccentComponent, AccentGetEvent>(OnAccentGet);
    }

    public string Accentuate(string message, FrenchAccentComponent component)
    {
        var msg = message;

        msg = _replacement.ApplyReplacements(msg, "french");

<<<<<<< HEAD
        // replaces th with dz
        msg = RegexTh.Replace(msg, "'z");

        // removes the letter h from the start of words.
        msg = RegexStartH.Replace(msg, "'");

        // spaces out ! ? : and ;.
        msg = RegexSpacePunctuation.Replace(msg, " $&");
=======
        // replaces th with dz 
        msg = Regex.Replace(msg, @"th", "'z", RegexOptions.IgnoreCase);

        // removes the letter h from the start of words.
        msg = Regex.Replace(msg, @"(?<!\w)[h]", "'", RegexOptions.IgnoreCase);

        // spaces out ! ? : and ;.
        msg = Regex.Replace(msg, @"(?<=\w\w)!(?!\w)", " !", RegexOptions.IgnoreCase);
        msg = Regex.Replace(msg, @"(?<=\w\w)[?](?!\w)", " ?", RegexOptions.IgnoreCase);
        msg = Regex.Replace(msg, @"(?<=\w\w)[;](?!\w)", " ;", RegexOptions.IgnoreCase);
        msg = Regex.Replace(msg, @"(?<=\w\w)[:](?!\w)", " :", RegexOptions.IgnoreCase);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

        return msg;
    }

    private void OnAccentGet(EntityUid uid, FrenchAccentComponent component, AccentGetEvent args)
    {
        args.Message = Accentuate(args.Message, component);
    }
}
