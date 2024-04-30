using Content.Shared._EstacaoPirata.Xenobiology.SlimeGrinder;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Server._EstacaoPirata.Chemistry.ReactionEffects;

public sealed partial class IncreaseExtractYield : ReagentEffect
{
    [DataField("rate")]
    public float Rate = 15f;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        return null;
    }

    public override void Effect(ReagentEffectArgs args)
    {
        if (!args.EntityManager.TrySystem<SharedContainerSystem>(out var containerSystem))
            return;

        if (!args.EntityManager.TryGetComponent<SlimeGrindableComponent>(args.SolutionEntity, out var grindable))
            return;

        // 1u = 100
        var quantityScaledDown = args.Quantity.Value / 100f;

        var val = quantityScaledDown / Rate;

        grindable.Yield += val;

        if (grindable.Yield >= 4f)
        {
            grindable.Yield = 4f;
        }
    }
}
