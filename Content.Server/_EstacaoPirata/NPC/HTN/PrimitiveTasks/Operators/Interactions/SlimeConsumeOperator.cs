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
using Serilog;

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
            { IdleKey,  1f }
        });
    }

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        if (!blackboard.TryGetValue<EntityUid>(Target, out var target, _entManager))
        {
            return HTNOperatorStatus.Failed;
        }
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        var count = 0;
        var slimeLeapingSystem = _entManager.System<SharedSlimeLeapingSystem>();

        _entManager.TryGetComponent<SlimeFeedingComponent>(owner, out var slimeFeedingComponent);
        _entManager.TryGetComponent<HungerComponent>(owner, out var hungerComponent);
        _entManager.TryGetComponent<SlimeFoodComponent>(target, out var slimeFoodComponent);
        _entManager.TryGetComponent<SlimeFeedingIncapacitatedComponent>(target, out var slimeFeedingIncapacitatedComponent);

        if (slimeFeedingComponent == null)
            return HTNOperatorStatus.Failed;


        if (slimeFeedingIncapacitatedComponent != null)
        {
            if (slimeFeedingIncapacitatedComponent.Attacker != null && slimeFeedingIncapacitatedComponent.Attacker != owner)
            {
                return HTNOperatorStatus.Finished;
            }
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

            if (slimeFeedingComponent.Victim != null)
            {
                return HTNOperatorStatus.Continuing;
            }
        }

        if (_entManager.TryGetComponent<DoAfterComponent>(owner, out var doAfter))
        {
            count = doAfter.DoAfters.Count;
        }

        var result = slimeLeapingSystem.LeapToTarget(owner, target);

        // Interaction started a doafter so set the idle time to it.
        if (result && doAfter != null && doAfter.DoAfters.Count > 0)
        {
            var wait = doAfter.DoAfters.First().Value.Args.Delay;
            blackboard.SetValue(IdleKey, (float) wait.TotalSeconds + (float) wait.TotalSeconds);
        }
        else
        {
            blackboard.SetValue(IdleKey, 4f);
        }

        // if (slimeFeedingComponent.Victim == null)
        // {
        //     return HTNOperatorStatus.Finished;
        // }

        return result ? HTNOperatorStatus.Finished : HTNOperatorStatus.Failed;
    }
}
