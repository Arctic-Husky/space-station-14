using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Content.Server._EstacaoPirata.Xenobiology.SlimeFeeding;
using Content.Server.NPC;
using Content.Server.NPC.HTN;
using Content.Server.NPC.HTN.PrimitiveTasks;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeFeeding;
using Content.Shared.DoAfter;
using Content.Shared.Nutrition.Components;

namespace Content.Server._EstacaoPirata.NPC.HTN.PrimitiveTasks.Operators.Interactions;

/// <summary>
/// This handles...
/// </summary>
public sealed partial class SlimeConsumeOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    [DataField("target")]
    public string Target = "Target";

    [DataField("hungerThreshold")]
    public HungerThreshold HungerThreshold = HungerThreshold.Overfed;

    /// <summary>
    /// If this alt-interaction started a do_after where does the key get stored.
    /// </summary>
    [DataField("idleKey")]
    public string IdleKey = "IdleTime";

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard, CancellationToken cancelToken)
    {
        return new(true, new Dictionary<string, object>()
        {
            { IdleKey, 1f }
        });
    }

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        if (!blackboard.TryGetValue<EntityUid>(Target, out var target, _entManager))
        {
            return HTNOperatorStatus.Failed;
        }

        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        var slimeLeapingSystem = _entManager.System<SharedSlimeLeapingSystem>();

        _entManager.TryGetComponent<SlimeFeedingComponent>(owner, out var slimeFeedingComponent);
        _entManager.TryGetComponent<HungerComponent>(owner, out var hungerComponent);

        _entManager.TryGetComponent<SlimeFoodComponent>(target, out var slimeFoodComponent);

        if (slimeFeedingComponent != null)
        {
            slimeLeapingSystem.LeapToTarget(owner, target);

            if (slimeFeedingComponent.Victim == null)
            {
                return HTNOperatorStatus.Continuing;
            }

            _entManager.TryGetComponent<DoAfterComponent>(owner, out var doAfter);

            if (doAfter != null)
            {
                if (!doAfter.DoAfters.Any())
                {
                    return HTNOperatorStatus.Continuing;
                }

                var wait = doAfter.DoAfters.First().Value.Args.Delay;
                blackboard.SetValue(IdleKey, (float) wait.TotalSeconds + 0.5f);
            }

            // Cabou comida do alvo
            if (slimeFoodComponent != null)
            {
                if (slimeFoodComponent.Remaining <= 0)
                {
                    return HTNOperatorStatus.Finished;
                }
            }

            // Ta de buchin chei
            if (!slimeFeedingComponent.StomachAvailable)
            {
                return HTNOperatorStatus.Finished;
            }

            if (hungerComponent != null)
            {
                // Ainda esta com fome
                if (hungerComponent.CurrentThreshold >= HungerThreshold)
                {
                    return HTNOperatorStatus.Finished;
                }

                return HTNOperatorStatus.Continuing;
            }
        }

        return HTNOperatorStatus.Failed;
    }
}
