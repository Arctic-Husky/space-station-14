namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;

/// <summary>
/// This is used for controlling what reactions will happen when the slime is injected with a reagent
/// </summary>
[RegisterComponent]
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
}

[DataDefinition]
public sealed partial class SlimeExtractReactionEntry
{

    [DataField("method", required: true)]
    public SlimeReactionMethod Method;

    [DataField("effects", required: true)]
    public List<SlimeReagentEffect> Effects = default!;
    // [DataField("groups", readOnly: true, serverOnly: true,
    //     customTypeSerializer:typeof(PrototypeIdDictionarySerializer<HashSet<ReactionMethod>, ReactiveGroupPrototype>))]
    // public Dictionary<string, HashSet<string>>? ReactiveGroups { get; private set; }
}
