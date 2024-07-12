using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;

/// <summary>
/// This is used for controlling what reactions will happen when the slime is injected with a reagent
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SlimeReactionComponent : Component
{
    // Tipos de reações:
    // Criar entidade (item, gas, mob)
    // Encher o extract com algum reagente
    // Mexer na IA dos mobs
    // Dar um componente especifico para alguma entidade (ex: fazer o extract emitir luz)

    // Outlier: sepia slime: plasma, para o tempo por 15 segundos, vai tomar no cu isso nao vou fazer

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

    [DataField("useInHand"), ViewVariables]
    public bool UseInHand = false;

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
    [DataField("method", required: true)]
    public string Method;

    // Ter uma variavel List<string> que contera os reagentes

    [DataField("effects", required: true)]
    public List<SlimeReagentEffect> Effects = default!;

    [DataField("sound")]
    public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/Effects/Chemistry/bubbles.ogg",
        new AudioParams
    {
        Volume = -10
    });
}
