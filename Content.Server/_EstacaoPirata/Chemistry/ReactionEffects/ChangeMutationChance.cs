using Content.Server._EstacaoPirata.Xenobiology;
using Content.Shared._EstacaoPirata.Xenobiology.Meiosis;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Server._EstacaoPirata.Chemistry.ReactionEffects;

// TODO: mudar isso pra aceitar decreases
public sealed partial class ChangeMutationChance : ReagentEffect
{
    [DataField("rate")]
    public FixedPoint2 Rate = FixedPoint2.FromCents(1500);

    [DataField("increase")]
    public bool Increase = true;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        return null;
    }

    public override void Effect(ReagentEffectArgs args)
    {
        if (!args.EntityManager.TrySystem<MeiosisSystem>(out var meiosisSystem))
            return;

        if (!args.EntityManager.TryGetComponent<MeiosisComponent>(args.SolutionEntity, out var meiosis))
            return;

        meiosis.AccumulatedMutagen += args.Quantity;

        if (meiosis.AccumulatedMutagen < Rate)
            return;

        var chance = meiosis.MutationChance;

        MeiosisThreshold newChance;

        if (Increase)
        {
            if (chance == MeiosisThreshold.Max)
                return;

            newChance = meiosisSystem.EnumNext(chance);
        }
        else
        {
            if (chance == MeiosisThreshold.Low)
                return;

            newChance = meiosisSystem.EnumPrevious(chance);
        }

        meiosis.MutationChance = newChance;

        meiosis.AccumulatedMutagen = FixedPoint2.Zero;

        meiosisSystem.PickMutations(meiosis);
    }
}
