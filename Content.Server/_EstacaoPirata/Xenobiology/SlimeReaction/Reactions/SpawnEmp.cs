using Content.Server.Emp;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeReaction.Reactions;

public sealed partial class SpawnEmp : SlimeReagentEffect
{
    [DataField("range")]
    public float Range = 4f;

    [DataField("energyConsumption")]
    public float EnergyConsumption = 50000;

    [DataField("duration")]
    public float Duration = 60f;

    public override bool Effect(SlimeReagentEffectArgs args)
    {
        var emp = args.EntityManager.System<EmpSystem>();
        var transformSystem = args.EntityManager.System<TransformSystem>();

        var extractEntity = args.ExtractEntity;

        var mapCoordinates = transformSystem.GetMapCoordinates(extractEntity);

        emp.EmpPulse(mapCoordinates, Range, EnergyConsumption, Duration);

        return true;
    }

    public override float TimeNeeded()
    {
        return 0f;
    }

    public override bool SpendOnUse()
    {
        return true;
    }

    public override string GetReactionMessage()
    {
        return "extract-explosion";
    }
}
