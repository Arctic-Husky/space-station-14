using Content.Server.Interaction;
using Content.Server.NPC;
using Content.Server.NPC.HTN.Preconditions;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeFeeding;

namespace Content.Server._EstacaoPirata.NPC.HTN.Preconditions;

public sealed partial class TargetNotBadSlimeFoodPrecondition : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    [DataField("targetKey")]
    public string TargetKey = "Target";

    public override bool IsMet(NPCBlackboard blackboard)
    {
        if (!blackboard.TryGetValue<EntityUid>(TargetKey, out var target, _entManager))
            return false;

        return !_entManager.HasComponent<SlimeBadFoodComponent>(target);
    }
}
