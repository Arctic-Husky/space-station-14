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

    public string SolutionName = "slimeExtract";

    public bool ExtractJustSpawned = true;
}

[DataDefinition]
public sealed partial class SlimeExtractReactionEntry
{
    [DataField("method", required: true)]
    public SlimeReactionMethod Method;

    [DataField("effects", required: true)]
    public List<SlimeReagentEffect> Effects = default!;

    [DataField("sound")]
    public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/Effects/Chemistry/bubbles.ogg",
        new AudioParams
    {
        Volume = -10
    });
}
