using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;

/// <summary>
/// This is used for controlling what reactions will happen when the slime is injected with a reagent
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SlimeReactionComponent : Component
{
    [DataField("reactions", true, serverOnly: true)]
    public List<SlimeExtractReactionEntry>? Reactions;

    [ViewVariables, AutoNetworkedField]
    public bool Spent;

    [DataField("reactionSound")]
    public SoundSpecifier? ReactionSound = new SoundPathSpecifier("/Audio/Effects/Chemistry/bubbles.ogg",
        new AudioParams
    {
        Volume = -10
    });

    [DataField("color")]
    public string Color = default!;

    public string SolutionName = "slimeExtract";

    public bool ExtractJustSpawned = true;

    public List<ReagentQuantity> Contents = new();
}

// TODO o method tem que ser de Interaction ou Substancia, ai se for substancia, ai sim especificar qual

// Melhor: criar prototipos para cada crossbreed, ex id:BurningGreySlimeExtract
// Criar um tipo de ativacao do slime que necessite apertar Z para interagir, alem de injetar o agente quimico
// Criar uma maquina que lida com crossbreeding. Tera dois slots, o de 1 slime e de ate 10 extracts. Um botao para ativar o crossbreed

[DataDefinition]
public sealed partial class SlimeExtractReactionEntry
{
    // UseInHand deve estar aqui, um Method tem que ser Interaction ou useinhand. Inclusive, aqui tem que ser uma lista
    [DataField("reagent")]
    public string Reagent = "None"; // Reagentes normais e None

    [DataField("interaction")]
    public bool Interaction = false; // TODO usar isso pra bloquear interaction ou permitir

    [DataField("crossbreed")]
    public string? Crossbreed;

    [DataField("effects", required: true)]
    public List<SlimeReagentEffect> Effects = default!;

    [DataField("sound")]
    public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/Effects/Chemistry/bubbles.ogg",
        new AudioParams
    {
        Volume = -10
    });
}
