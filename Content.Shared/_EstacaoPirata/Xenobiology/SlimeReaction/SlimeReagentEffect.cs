using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;

[ImplicitDataDefinitionForInheritors]
[MeansImplicitUse]
public abstract partial class SlimeReagentEffect
{
    [JsonPropertyName("id")] private protected string _id => this.GetType().Name;

    public abstract void Effect(SlimeReagentEffectArgs args);
}

public enum SlimeReactionMethod
{
    Plasma,
    Blood,
    Water
}

public readonly record struct SlimeReagentEffectArgs(
    string EntityToSpawn,
    EntityUid? TargetEntity,
    float Quantity,
    IEntityManager EntityManager,
    float Scale
);
