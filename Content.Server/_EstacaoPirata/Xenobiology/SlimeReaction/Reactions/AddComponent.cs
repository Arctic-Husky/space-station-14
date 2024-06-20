using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeReaction.Reactions;

public sealed partial class AddComponent : SlimeReagentEffect
{
    [DataField("comp")]
    public ComponentRegistry Comp = new();

    public override bool Effect(SlimeReagentEffectArgs args)
    {
        foreach (var comp in Comp)
        {
            var componentToAdd = comp.Value.Component;

            args.EntityManager.AddComponent(args.ExtractEntity, componentToAdd);
        }

        return true;
    }

    public override float TimeNeeded()
    {
        return 0f;
    }

    public override bool SpendOnUse()
    {
        return false;
    }
}
