using Content.Shared.Humanoid;
<<<<<<< HEAD
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
=======
using Robust.Shared.Player;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

namespace Content.Shared.Preferences
{
    public interface ICharacterProfile
    {
        string Name { get; }

        ICharacterAppearance CharacterAppearance { get; }

        bool MemberwiseEquals(ICharacterProfile other);

        /// <summary>
        ///     Makes this profile valid so there's no bad data like negative ages.
        /// </summary>
        void EnsureValid(ICommonSession session, IDependencyCollection collection);

        /// <summary>
        /// Gets a copy of this profile that has <see cref="EnsureValid"/> applied, i.e. no invalid data.
        /// </summary>
        ICharacterProfile Validated(ICommonSession session, IDependencyCollection collection);
    }
}
