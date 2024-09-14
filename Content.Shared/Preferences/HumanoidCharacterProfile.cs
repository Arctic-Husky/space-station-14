using System.Linq;
using System.Text.RegularExpressions;
using Content.Shared.CCVar;
using Content.Shared.Clothing.Loadouts.Prototypes;
using Content.Shared.GameTicking;
using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Prototypes;
<<<<<<< HEAD
using Content.Shared.Preferences.Loadouts;
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
using Content.Shared.Roles;
using Content.Shared.Traits;
using Robust.Shared.Collections;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Preferences
{
    /// <summary>
    /// Character profile. Looks immutable, but uses non-immutable semantics internally for serialization/code sanity purposes.
    /// </summary>
    [DataDefinition]
    [Serializable, NetSerializable]
    public sealed partial class HumanoidCharacterProfile : ICharacterProfile
    {
<<<<<<< HEAD
        private static readonly Regex RestrictedNameRegex = new("[^A-Za-z0-9çÀÁÉÊÍÓÔÚÃÕàáéêíóôúãõ -]");
        private static readonly Regex ICNameCaseRegex = new(@"^(?<word>\w)|\b(?<word>\w)(?=\w*$)");


        public const int MaxNameLength = 32;
        public const int MaxDescLength = 512;

        /// <summary>
        /// Job preferences for initial spawn.
        /// </summary>
        [DataField]
        private Dictionary<string, JobPriority> _jobPriorities = new()
        {
            {
                SharedGameTicker.FallbackOverflowJob, JobPriority.High
            }
        };

        /// <summary>
        /// Antags we have opted in to.
        /// </summary>
        [DataField]
        private HashSet<string> _antagPreferences = new();

        /// <summary>
        /// Enabled traits.
        /// </summary>
        [DataField]
        private HashSet<string> _traitPreferences = new();

        /// <summary>
        /// <see cref="_loadouts"/>
        /// </summary>
        public IReadOnlyDictionary<string, RoleLoadout> Loadouts => _loadouts;

        [DataField]
        private Dictionary<string, RoleLoadout> _loadouts = new();

        [DataField]
        public string Name { get; set; } = "John Doe";

        /// <summary>
        /// Detailed text that can appear for the character if <see cref="CCVars.FlavorText"/> is enabled.
        /// </summary>
        [DataField]
        public string FlavorText { get; set; } = string.Empty;

        /// <summary>
        /// Associated <see cref="SpeciesPrototype"/> for this profile.
        /// </summary>
        [DataField]
        public string Species { get; set; } = SharedHumanoidAppearanceSystem.DefaultSpecies;

        [DataField]
        public int Age { get; set; } = 18;

        [DataField]
        public Sex Sex { get; private set; } = Sex.Male;

        [DataField]
        public Gender Gender { get; private set; } = Gender.Male;

        /// <summary>
        /// <see cref="Appearance"/>
        /// </summary>
        public ICharacterAppearance CharacterAppearance => Appearance;

        /// <summary>
        /// Stores markings, eye colors, etc for the profile.
        /// </summary>
        [DataField]
        public HumanoidCharacterAppearance Appearance { get; set; } = new();

        /// <summary>
        /// When spawning into a round what's the preferred spot to spawn.
        /// </summary>
        [DataField]
        public SpawnPriorityPreference SpawnPriority { get; private set; } = SpawnPriorityPreference.None;

        /// <summary>
        /// <see cref="_jobPriorities"/>
        /// </summary>
        public IReadOnlyDictionary<string, JobPriority> JobPriorities => _jobPriorities;

        /// <summary>
        /// <see cref="_antagPreferences"/>
        /// </summary>
        public IReadOnlySet<string> AntagPreferences => _antagPreferences;

        /// <summary>
        /// <see cref="_traitPreferences"/>
        /// </summary>
        public IReadOnlySet<string> TraitPreferences => _traitPreferences;

        /// <summary>
        /// If we're unable to get one of our preferred jobs do we spawn as a fallback job or do we stay in lobby.
        /// </summary>
        [DataField]
        public PreferenceUnavailableMode PreferenceUnavailable { get; private set; } =
            PreferenceUnavailableMode.SpawnAsOverflow;
=======
        public const int MaxNameLength = 48;
        public const int MaxDescLength = 1024;

        private readonly Dictionary<string, JobPriority> _jobPriorities;
        private readonly List<string> _antagPreferences;
        private readonly List<string> _traitPreferences;
        private readonly List<string> _loadoutPreferences;

        private HumanoidCharacterProfile(
            string name,
            string flavortext,
            string species,
            float height,
            float width,
            int age,
            Sex sex,
            Gender gender,
            HumanoidCharacterAppearance appearance,
            ClothingPreference clothing,
            BackpackPreference backpack,
            SpawnPriorityPreference spawnPriority,
            Dictionary<string, JobPriority> jobPriorities,
            PreferenceUnavailableMode preferenceUnavailable,
            List<string> antagPreferences,
            List<string> traitPreferences,
            List<string> loadoutPreferences)
        {
            Name = name;
            FlavorText = flavortext;
            Species = species;
            Height = height;
            Width = width;
            Age = age;
            Sex = sex;
            Gender = gender;
            Appearance = appearance;
            Clothing = clothing;
            Backpack = backpack;
            SpawnPriority = spawnPriority;
            _jobPriorities = jobPriorities;
            PreferenceUnavailable = preferenceUnavailable;
            _antagPreferences = antagPreferences;
            _traitPreferences = traitPreferences;
            _loadoutPreferences = loadoutPreferences;
        }

        /// <summary>Copy constructor but with overridable references (to prevent useless copies)</summary>
        private HumanoidCharacterProfile(
            HumanoidCharacterProfile other,
            Dictionary<string, JobPriority> jobPriorities,
            List<string> antagPreferences,
            List<string> traitPreferences,
            List<string> loadoutPreferences)
            : this(other.Name, other.FlavorText, other.Species, other.Height, other.Width, other.Age, other.Sex, other.Gender, other.Appearance,
                other.Clothing, other.Backpack, other.SpawnPriority, jobPriorities, other.PreferenceUnavailable,
                antagPreferences, traitPreferences, loadoutPreferences)
        {
        }

        /// <summary>Copy constructor</summary>
        private HumanoidCharacterProfile(HumanoidCharacterProfile other)
            : this(other, new Dictionary<string, JobPriority>(other.JobPriorities),
                new List<string>(other.AntagPreferences), new List<string>(other.TraitPreferences),
                new List<string>(other.LoadoutPreferences))
        {
        }
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

        public HumanoidCharacterProfile(
            string name,
            string flavortext,
            string species,
            float height,
            float width,
            int age,
            Sex sex,
            Gender gender,
            HumanoidCharacterAppearance appearance,
<<<<<<< HEAD
            SpawnPriorityPreference spawnPriority,
            Dictionary<string, JobPriority> jobPriorities,
            PreferenceUnavailableMode preferenceUnavailable,
            HashSet<string> antagPreferences,
            HashSet<string> traitPreferences,
            Dictionary<string, RoleLoadout> loadouts)
        {
            Name = name;
            FlavorText = flavortext;
            Species = species;
            Age = age;
            Sex = sex;
            Gender = gender;
            Appearance = appearance;
            SpawnPriority = spawnPriority;
            _jobPriorities = jobPriorities;
            PreferenceUnavailable = preferenceUnavailable;
            _antagPreferences = antagPreferences;
            _traitPreferences = traitPreferences;
            _loadouts = loadouts;
        }

        /// <summary>Copy constructor</summary>
        public HumanoidCharacterProfile(HumanoidCharacterProfile other)
            : this(other.Name,
                other.FlavorText,
                other.Species,
                other.Age,
                other.Sex,
                other.Gender,
                other.Appearance.Clone(),
                other.SpawnPriority,
                new Dictionary<string, JobPriority>(other.JobPriorities),
                other.PreferenceUnavailable,
                new HashSet<string>(other.AntagPreferences),
                new HashSet<string>(other.TraitPreferences),
                new Dictionary<string, RoleLoadout>(other.Loadouts))
=======
            ClothingPreference clothing,
            BackpackPreference backpack,
            SpawnPriorityPreference spawnPriority,
            IReadOnlyDictionary<string, JobPriority> jobPriorities,
            PreferenceUnavailableMode preferenceUnavailable,
            IReadOnlyList<string> antagPreferences,
            IReadOnlyList<string> traitPreferences,
            IReadOnlyList<string> loadoutPreferences)
            : this(name, flavortext, species, height, width, age, sex, gender, appearance, clothing, backpack, spawnPriority,
                new Dictionary<string, JobPriority>(jobPriorities), preferenceUnavailable,
                new List<string>(antagPreferences), new List<string>(traitPreferences),
                new List<string>(loadoutPreferences))
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        {
        }

        /// <summary>
        ///     Get the default humanoid character profile, using internal constant values.
        ///     Defaults to <see cref="SharedHumanoidAppearanceSystem.DefaultSpecies"/> for the species.
        /// </summary>
        /// <returns></returns>
<<<<<<< HEAD
        public HumanoidCharacterProfile()
=======
        public HumanoidCharacterProfile() : this(
            "John Doe",
            "",
            SharedHumanoidAppearanceSystem.DefaultSpecies,
            1f,
            1f,
            18,
            Sex.Male,
            Gender.Male,
            new HumanoidCharacterAppearance(),
            ClothingPreference.Jumpsuit,
            BackpackPreference.Backpack,
            SpawnPriorityPreference.None,
            new Dictionary<string, JobPriority>
            {
                {SharedGameTicker.FallbackOverflowJob, JobPriority.High}
            },
            PreferenceUnavailableMode.SpawnAsOverflow,
            new List<string>(),
            new List<string>(),
            new List<string>())
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        {
        }

        /// <summary>
        ///     Return a default character profile, based on species.
        /// </summary>
        /// <param name="species">The species to use in this default profile. The default species is <see cref="SharedHumanoidAppearanceSystem.DefaultSpecies"/>.</param>
        /// <returns>Humanoid character profile with default settings.</returns>
        public static HumanoidCharacterProfile DefaultWithSpecies(string species = SharedHumanoidAppearanceSystem.DefaultSpecies)
        {
<<<<<<< HEAD
            return new()
            {
                Species = species,
            };
=======
            return new(
                "John Doe",
                "",
                species,
                1f,
                1f,
                18,
                Sex.Male,
                Gender.Male,
                HumanoidCharacterAppearance.DefaultWithSpecies(species),
                ClothingPreference.Jumpsuit,
                BackpackPreference.Backpack,
                SpawnPriorityPreference.None,
                new Dictionary<string, JobPriority>
                {
                    {SharedGameTicker.FallbackOverflowJob, JobPriority.High}
                },
                PreferenceUnavailableMode.SpawnAsOverflow,
                new List<string>(),
                new List<string>(),
                new List<string>());
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        }

        // TODO: This should eventually not be a visual change only.
        public static HumanoidCharacterProfile Random(HashSet<string>? ignoredSpecies = null)
        {
            var prototypeManager = IoCManager.Resolve<IPrototypeManager>();
            var random = IoCManager.Resolve<IRobustRandom>();

            var species = random.Pick(prototypeManager
                .EnumeratePrototypes<SpeciesPrototype>()
                .Where(x => ignoredSpecies == null ? x.RoundStart : x.RoundStart && !ignoredSpecies.Contains(x.ID))
                .ToArray()
            ).ID;

            return RandomWithSpecies(species);
        }

        public static HumanoidCharacterProfile RandomWithSpecies(string species = SharedHumanoidAppearanceSystem.DefaultSpecies)
        {
            var prototypeManager = IoCManager.Resolve<IPrototypeManager>();
            var random = IoCManager.Resolve<IRobustRandom>();

            var sex = Sex.Unsexed;
            var age = 18;
            var height = 1f;
            var width = 1f;
            if (prototypeManager.TryIndex<SpeciesPrototype>(species, out var speciesPrototype))
            {
                sex = random.Pick(speciesPrototype.Sexes);
                age = random.Next(speciesPrototype.MinAge, speciesPrototype.OldAge); // people don't look and keep making 119 year old characters with zero rp, cap it at middle aged
                height = random.NextFloat(speciesPrototype.MinHeight, speciesPrototype.MaxHeight);
                width = random.NextFloat(speciesPrototype.MinWidth, speciesPrototype.MaxWidth);
            }

            var gender = Gender.Epicene;

            switch (sex)
            {
                case Sex.Male:
                    gender = Gender.Male;
                    break;
                case Sex.Female:
                    gender = Gender.Female;
                    break;
            }

            var name = GetName(species, gender);

<<<<<<< HEAD
            return new HumanoidCharacterProfile()
            {
                Name = name,
                Sex = sex,
                Age = age,
                Gender = gender,
                Species = species,
                Appearance = HumanoidCharacterAppearance.Random(species, sex),
            };
        }

=======
            return new HumanoidCharacterProfile(name, "", species, height, width, age, sex, gender,
                HumanoidCharacterAppearance.Random(species, sex), ClothingPreference.Jumpsuit,
                BackpackPreference.Backpack, SpawnPriorityPreference.None,
                new Dictionary<string, JobPriority>
                {
                    {SharedGameTicker.FallbackOverflowJob, JobPriority.High},
                }, PreferenceUnavailableMode.StayInLobby, new List<string>(), new List<string>(), new List<string>());
        }

        public string Name { get; private set; }
        public string FlavorText { get; private set; }
        [DataField("species")]
        public string Species { get; private set; }

        [DataField("height")]
        public float Height { get; private set; }

        [DataField("width")]
        public float Width { get; private set; }

        [DataField("age")]
        public int Age { get; private set; }

        [DataField("sex")]
        public Sex Sex { get; private set; }

        [DataField("gender")]
        public Gender Gender { get; private set; }

        public ICharacterAppearance CharacterAppearance => Appearance;

        [DataField("appearance")]
        public HumanoidCharacterAppearance Appearance { get; private set; }
        public ClothingPreference Clothing { get; private set; }
        public BackpackPreference Backpack { get; private set; }
        public SpawnPriorityPreference SpawnPriority { get; private set; }
        public IReadOnlyDictionary<string, JobPriority> JobPriorities => _jobPriorities;
        public IReadOnlyList<string> AntagPreferences => _antagPreferences;
        public IReadOnlyList<string> TraitPreferences => _traitPreferences;
        public IReadOnlyList<string> LoadoutPreferences => _loadoutPreferences;
        public PreferenceUnavailableMode PreferenceUnavailable { get; private set; }

>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        public HumanoidCharacterProfile WithName(string name)
        {
            return new(this) { Name = name };
        }

        public HumanoidCharacterProfile WithFlavorText(string flavorText)
        {
            return new(this) { FlavorText = flavorText };
        }

        public HumanoidCharacterProfile WithAge(int age)
        {
            return new(this) { Age = age };
        }

        public HumanoidCharacterProfile WithSex(Sex sex)
        {
            return new(this) { Sex = sex };
        }

        public HumanoidCharacterProfile WithGender(Gender gender)
        {
            return new(this) { Gender = gender };
        }

        public HumanoidCharacterProfile WithSpecies(string species)
        {
            return new(this) { Species = species };
        }

        public HumanoidCharacterProfile WithHeight(float height)
        {
            return new(this) { Height = height };
        }

        public HumanoidCharacterProfile WithWidth(float width)
        {
            return new(this) { Width = width };
        }

        public HumanoidCharacterProfile WithCharacterAppearance(HumanoidCharacterAppearance appearance)
        {
            return new(this) { Appearance = appearance };
        }

        public HumanoidCharacterProfile WithSpawnPriorityPreference(SpawnPriorityPreference spawnPriority)
        {
            return new(this) { SpawnPriority = spawnPriority };
        }
<<<<<<< HEAD

        public HumanoidCharacterProfile WithJobPriorities(IEnumerable<KeyValuePair<string, JobPriority>> jobPriorities)
        {
            return new(this)
            {
                _jobPriorities = new Dictionary<string, JobPriority>(jobPriorities),
            };
=======
        public HumanoidCharacterProfile WithSpawnPriorityPreference(SpawnPriorityPreference spawnPriority)
        {
            return new(this) { SpawnPriority = spawnPriority };
        }
        public HumanoidCharacterProfile WithJobPriorities(IEnumerable<KeyValuePair<string, JobPriority>> jobPriorities)
        {
            return new(this, new Dictionary<string, JobPriority>(jobPriorities), _antagPreferences, _traitPreferences,
                _loadoutPreferences);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        }

        public HumanoidCharacterProfile WithJobPriority(string jobId, JobPriority priority)
        {
            var dictionary = new Dictionary<string, JobPriority>(_jobPriorities);
            if (priority == JobPriority.Never)
            {
                dictionary.Remove(jobId);
            }
            else
            {
                dictionary[jobId] = priority;
            }
<<<<<<< HEAD

            return new(this)
            {
                _jobPriorities = dictionary,
            };
=======
            return new(this, dictionary, _antagPreferences, _traitPreferences, _loadoutPreferences);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        }

        public HumanoidCharacterProfile WithPreferenceUnavailable(PreferenceUnavailableMode mode)
        {
            return new(this) { PreferenceUnavailable = mode };
        }

        public HumanoidCharacterProfile WithAntagPreferences(IEnumerable<string> antagPreferences)
        {
<<<<<<< HEAD
            return new(this)
            {
                _antagPreferences = new HashSet<string>(antagPreferences),
            };
=======
            return new(this, _jobPriorities, new List<string>(antagPreferences), _traitPreferences,
                _loadoutPreferences);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        }

        public HumanoidCharacterProfile WithAntagPreference(string antagId, bool pref)
        {
<<<<<<< HEAD
            var list = new HashSet<string>(_antagPreferences);
            if (pref)
            {
                list.Add(antagId);
            }
            else
            {
                list.Remove(antagId);
            }

            return new(this)
            {
                _antagPreferences = list,
            };
=======
            var list = new List<string>(_antagPreferences);
            if (pref)
            {
                if (!list.Contains(antagId))
                {
                    list.Add(antagId);
                }
            }
            else
            {
                if (list.Contains(antagId))
                {
                    list.Remove(antagId);
                }
            }
            return new(this, _jobPriorities, list, _traitPreferences, _loadoutPreferences);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        }

        public HumanoidCharacterProfile WithTraitPreference(string traitId, bool pref)
        {
            var list = new HashSet<string>(_traitPreferences);

<<<<<<< HEAD
            if (pref)
            {
                list.Add(traitId);
            }
            else
            {
                list.Remove(traitId);
            }

            return new(this)
            {
                _traitPreferences = list,
            };
=======
            // TODO: Maybe just refactor this to HashSet? Same with _antagPreferences
            if (pref)
            {
                if (!list.Contains(traitId))
                {
                    list.Add(traitId);
                }
            }
            else
            {
                if (list.Contains(traitId))
                {
                    list.Remove(traitId);
                }
            }
            return new(this, _jobPriorities, _antagPreferences, list, _loadoutPreferences);
        }

        public HumanoidCharacterProfile WithLoadoutPreference(string loadoutId, bool pref)
        {
            var list = new List<string>(_loadoutPreferences);

            if(pref)
            {
                if(!list.Contains(loadoutId))
                {
                    list.Add(loadoutId);
                }
            }
            else
            {
                if(list.Contains(loadoutId))
                {
                    list.Remove(loadoutId);
                }
            }
            return new(this, _jobPriorities, _antagPreferences, _traitPreferences, list);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        }

        public string Summary =>
            Loc.GetString(
                "humanoid-character-profile-summary",
                ("name", Name),
                ("gender", Gender.ToString().ToLowerInvariant()),
                ("age", Age)
            );

        public bool MemberwiseEquals(ICharacterProfile maybeOther)
        {
<<<<<<< HEAD
            if (maybeOther is not HumanoidCharacterProfile other) return false;
            if (Name != other.Name) return false;
            if (Age != other.Age) return false;
            if (Sex != other.Sex) return false;
            if (Gender != other.Gender) return false;
            if (Species != other.Species) return false;
            if (PreferenceUnavailable != other.PreferenceUnavailable) return false;
            if (SpawnPriority != other.SpawnPriority) return false;
            if (!_jobPriorities.SequenceEqual(other._jobPriorities)) return false;
            if (!_antagPreferences.SequenceEqual(other._antagPreferences)) return false;
            if (!_traitPreferences.SequenceEqual(other._traitPreferences)) return false;
            if (!Loadouts.SequenceEqual(other.Loadouts)) return false;
=======
            if (maybeOther is not HumanoidCharacterProfile other
                || Name != other.Name
                || Age != other.Age
                || Height != other.Height
                || Width != other.Width
                || Sex != other.Sex
                || Gender != other.Gender
                || PreferenceUnavailable != other.PreferenceUnavailable
                || Clothing != other.Clothing
                || Backpack != other.Backpack
                || SpawnPriority != other.SpawnPriority
                || !_jobPriorities.SequenceEqual(other._jobPriorities)
                || !_antagPreferences.SequenceEqual(other._antagPreferences)
                || !_traitPreferences.SequenceEqual(other._traitPreferences)
                || !_loadoutPreferences.SequenceEqual(other._loadoutPreferences))
                return false;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            return Appearance.MemberwiseEquals(other.Appearance);
        }

        public void EnsureValid(ICommonSession session, IDependencyCollection collection)
        {
            var configManager = collection.Resolve<IConfigurationManager>();
            var prototypeManager = collection.Resolve<IPrototypeManager>();

            if (!prototypeManager.TryIndex<SpeciesPrototype>(Species, out var speciesPrototype) || speciesPrototype.RoundStart == false)
            {
                Species = SharedHumanoidAppearanceSystem.DefaultSpecies;
                speciesPrototype = prototypeManager.Index<SpeciesPrototype>(Species);
            }

            var sex = Sex switch
            {
                Sex.Male => Sex.Male,
                Sex.Female => Sex.Female,
                Sex.Unsexed => Sex.Unsexed,
                _ => Sex.Male // Invalid enum values.
            };

            // ensure the species can be that sex and their age fits the founds
            if (!speciesPrototype.Sexes.Contains(sex))
                sex = speciesPrototype.Sexes[0];

            var age = Math.Clamp(Age, speciesPrototype.MinAge, speciesPrototype.MaxAge);

            var gender = Gender switch
            {
                Gender.Epicene => Gender.Epicene,
                Gender.Female => Gender.Female,
                Gender.Male => Gender.Male,
                Gender.Neuter => Gender.Neuter,
                _ => Gender.Epicene // Invalid enum values.
            };

            string name;
            if (string.IsNullOrEmpty(Name))
            {
                name = GetName(Species, gender);
            }
            else if (Name.Length > MaxNameLength)
            {
                name = Name[..MaxNameLength];
            }
            else
            {
                name = Name;
            }

            name = name.Trim();

            if (configManager.GetCVar(CCVars.RestrictedNames))
            {
<<<<<<< HEAD
                name = RestrictedNameRegex.Replace(name, string.Empty);
=======
                name = Regex.Replace(name, @"[^\u0030-\u0039,\u0041-\u005A,\u0061-\u007A,\u00C0-\u00D6,\u00D8-\u00F6,\u00F8-\u00FF,\u0100-\u017F, '.,-]", string.Empty);
                /*
                 * 0030-0039  Basic Latin: ASCII Digits
                 * 0041-005A  Basic Latin: Uppercase Latin Alphabet
                 * 0061-007A  Basic Latin: Lowercase Latin Alphabet
                 * 00C0-00D6  Latin-1 Supplement: Letters I
                 * 00D8-00F6  Latin-1 Supplement: Letters II
                 * 00F8-00FF  Latin-1 Supplement: Letters III
                 * 0100-017F  Latin Extended A: European Latin
                 */
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            }

            if (configManager.GetCVar(CCVars.ICNameCase))
            {
                // This regex replaces the first character of the first and last words of the name with their uppercase version
<<<<<<< HEAD
                name = ICNameCaseRegex.Replace(name, m => m.Groups["word"].Value.ToUpper());
=======
                name = Regex.Replace(name,
                    @"^(?<word>\w)|\b(?<word>\w)(?=\w*$)",
                    m => m.Groups["word"].Value.ToUpper());
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            }

            if (string.IsNullOrEmpty(name))
            {
                name = GetName(Species, gender);
            }

            string flavortext;
            if (FlavorText.Length > MaxDescLength)
            {
                flavortext = FormattedMessage.RemoveMarkup(FlavorText)[..MaxDescLength];
            }
            else
            {
                flavortext = FormattedMessage.RemoveMarkup(FlavorText);
            }

<<<<<<< HEAD
=======
            var height = Height;
            if (speciesPrototype != null)
                height = Math.Clamp(Height, speciesPrototype.MinHeight, speciesPrototype.MaxHeight);

            var width = Width;
            if (speciesPrototype != null)
                width = Math.Clamp(Width, speciesPrototype.MinWidth, speciesPrototype.MaxWidth);

>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            var appearance = HumanoidCharacterAppearance.EnsureValid(Appearance, Species, Sex);

            var prefsUnavailableMode = PreferenceUnavailable switch
            {
                PreferenceUnavailableMode.StayInLobby => PreferenceUnavailableMode.StayInLobby,
                PreferenceUnavailableMode.SpawnAsOverflow => PreferenceUnavailableMode.SpawnAsOverflow,
                _ => PreferenceUnavailableMode.StayInLobby // Invalid enum values.
            };

            var spawnPriority = SpawnPriority switch
            {
                SpawnPriorityPreference.None => SpawnPriorityPreference.None,
                SpawnPriorityPreference.Arrivals => SpawnPriorityPreference.Arrivals,
                SpawnPriorityPreference.Cryosleep => SpawnPriorityPreference.Cryosleep,
                _ => SpawnPriorityPreference.None // Invalid enum values.
            };

            var spawnPriority = SpawnPriority switch
            {
                SpawnPriorityPreference.None => SpawnPriorityPreference.None,
                SpawnPriorityPreference.Arrivals => SpawnPriorityPreference.Arrivals,
                SpawnPriorityPreference.Cryosleep => SpawnPriorityPreference.Cryosleep,
                _ => SpawnPriorityPreference.None // Invalid enum values.
            };

            var priorities = new Dictionary<string, JobPriority>(JobPriorities
                .Where(p => prototypeManager.TryIndex<JobPrototype>(p.Key, out var job) && job.SetPreference && p.Value switch
                {
                    JobPriority.Never => false, // Drop never since that's assumed default.
                    JobPriority.Low => true,
                    JobPriority.Medium => true,
                    JobPriority.High => true,
                    _ => false
                }));

            var antags = AntagPreferences
                .Where(id => prototypeManager.TryIndex<AntagPrototype>(id, out var antag) && antag.SetPreference)
                .ToList();

            var traits = TraitPreferences
                .Where(prototypeManager.HasIndex<TraitPrototype>)
                .ToList();

            var maxTraits = configManager.GetCVar(CCVars.GameTraitsMax);
            var currentTraits = 0;
            var traitPoints = configManager.GetCVar(CCVars.GameTraitsDefaultPoints);

            foreach (var trait in traits.OrderBy(t => -prototypeManager.Index<TraitPrototype>(t).Points).ToList())
            {
                var proto = prototypeManager.Index<TraitPrototype>(trait);

                if (traitPoints + proto.Points < 0 || currentTraits + 1 > maxTraits)
                    traits.Remove(trait);
                else
                {
                    traitPoints += proto.Points;
                    currentTraits++;
                }
            }


            var loadouts = LoadoutPreferences
                .Where(prototypeManager.HasIndex<LoadoutPrototype>)
                .ToList();

            var loadoutPoints = configManager.GetCVar(CCVars.GameLoadoutsPoints);
            var currentPoints = 0;

            foreach (var loadout in loadouts.ToList())
            {
                var proto = prototypeManager.Index<LoadoutPrototype>(loadout);

                if (currentPoints + proto.Cost > loadoutPoints)
                    loadouts.Remove(loadout);
                else
                    currentPoints += proto.Cost;
            }


            Name = name;
            FlavorText = flavortext;
            Age = age;
            Height = height;
            Width = width;
            Sex = sex;
            Gender = gender;
            Appearance = appearance;
<<<<<<< HEAD
=======
            Clothing = clothing;
            Backpack = backpack;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            SpawnPriority = spawnPriority;

            _jobPriorities.Clear();

            foreach (var (job, priority) in priorities)
            {
                _jobPriorities.Add(job, priority);
            }

            PreferenceUnavailable = prefsUnavailableMode;

            _antagPreferences.Clear();
            _antagPreferences.UnionWith(antags);

            _traitPreferences.Clear();
<<<<<<< HEAD
            _traitPreferences.UnionWith(traits);

            // Checks prototypes exist for all loadouts and dump / set to default if not.
            var toRemove = new ValueList<string>();

            foreach (var (roleName, loadouts) in _loadouts)
            {
                if (!prototypeManager.HasIndex<RoleLoadoutPrototype>(roleName))
                {
                    toRemove.Add(roleName);
                    continue;
                }

                loadouts.EnsureValid(this, session, collection);
            }

            foreach (var value in toRemove)
            {
                _loadouts.Remove(value);
            }
=======
            _traitPreferences.AddRange(traits);

            _loadoutPreferences.Clear();
            _loadoutPreferences.AddRange(loadouts);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        }

        public ICharacterProfile Validated(ICommonSession session, IDependencyCollection collection)
        {
            var profile = new HumanoidCharacterProfile(this);
            profile.EnsureValid(session, collection);
            return profile;
        }

        // sorry this is kind of weird and duplicated,
        /// working inside these non entity systems is a bit wack
        public static string GetName(string species, Gender gender)
        {
            var namingSystem = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<NamingSystem>();
            return namingSystem.GetName(species, gender);
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is HumanoidCharacterProfile other && Equals(other);
        }

        public override int GetHashCode()
        {
<<<<<<< HEAD
            var hashCode = new HashCode();
            hashCode.Add(_jobPriorities);
            hashCode.Add(_antagPreferences);
            hashCode.Add(_traitPreferences);
            hashCode.Add(_loadouts);
            hashCode.Add(Name);
            hashCode.Add(FlavorText);
            hashCode.Add(Species);
            hashCode.Add(Age);
            hashCode.Add((int)Sex);
            hashCode.Add((int)Gender);
            hashCode.Add(Appearance);
            hashCode.Add((int)SpawnPriority);
            hashCode.Add((int)PreferenceUnavailable);
            return hashCode.ToHashCode();
        }

        public void SetLoadout(RoleLoadout loadout)
        {
            _loadouts[loadout.Role.Id] = loadout;
        }

        public HumanoidCharacterProfile WithLoadout(RoleLoadout loadout)
        {
            // Deep copies so we don't modify the DB profile.
            var copied = new Dictionary<string, RoleLoadout>();

            foreach (var proto in _loadouts)
            {
                if (proto.Key == loadout.Role)
                    continue;

                copied[proto.Key] = proto.Value.Clone();
            }

            copied[loadout.Role] = loadout.Clone();
            var profile = Clone();
            profile._loadouts = copied;
            return profile;
        }

        public RoleLoadout GetLoadoutOrDefault(string id, ProtoId<SpeciesPrototype>? species, IEntityManager entManager, IPrototypeManager protoManager)
        {
            if (!_loadouts.TryGetValue(id, out var loadout))
            {
                loadout = new RoleLoadout(id);
                loadout.SetDefault(protoManager, force: true);
            }

            loadout.SetDefault(protoManager);
            return loadout;
        }

        public HumanoidCharacterProfile Clone()
        {
            return new HumanoidCharacterProfile(this);
=======
            return HashCode.Combine(
                HashCode.Combine(
                    Name,
                    Species,
                    Age,
                    Sex,
                    Gender,
                    Appearance,
                    Clothing,
                    Backpack
                ),
                HashCode.Combine(
                    SpawnPriority,
                    Height,
                    Width,
                    PreferenceUnavailable,
                    _jobPriorities,
                    _antagPreferences,
                    _traitPreferences,
                    _loadoutPreferences
                )
            );
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        }
    }
}
