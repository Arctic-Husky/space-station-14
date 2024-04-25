using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Robust.Shared.Containers;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;

public sealed partial class FillExtract : SlimeReagentEffect
{
    // TODO: mudar isto para uma lista de reagentes e suas quantidades, um dicionario
    [DataField("reagent")]
    public string Reagent;

    // TODO: lidar com esta quantidade minima
    [DataField("minReagentRequired")]
    public float MinReagentRequired;

    public override bool Effect(SlimeReagentEffectArgs args)
    {
        if (args.ExtractEntity == null)
            return false;

        var solutionContainerSystem = args.EntityManager.System<SharedSolutionContainerSystem>();

        var solutionContainerManagerComponent = args.EntityManager.GetComponent<SolutionContainerManagerComponent>(args.ExtractEntity.Value);
        var slimeReactionComponent = args.EntityManager.GetComponent<SlimeReactionComponent>(args.ExtractEntity.Value);

        var entity = new Entity<SolutionContainerManagerComponent?>(args.ExtractEntity.Value, solutionContainerManagerComponent);

        if (!solutionContainerSystem.TryGetSolution(entity, slimeReactionComponent.SolutionName, out var soln))
            return false;

        soln.Value.Comp.Solution.RemoveAllSolution();
        soln.Value.Comp.Solution.AddReagent(Reagent, soln.Value.Comp.Solution.MaxVolume);

        return true;
    }
}
