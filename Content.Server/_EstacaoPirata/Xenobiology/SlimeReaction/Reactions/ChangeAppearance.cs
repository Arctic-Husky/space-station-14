using Content.Server.Power.Components;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Shared.PowerCell;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeReaction.Reactions;

public sealed partial class ChangeAppearance : SlimeReagentEffect
{
    [DataField("visualEnum")]
    public Enum VisualEnum;

    [DataField("value")]
    public byte Value;

    // TODO: try to have some way of knowing if SetData worked or now
    public override bool Effect(SlimeReagentEffectArgs args)
    {
        var appearance = args.EntityManager.System<SharedAppearanceSystem>();

        appearance.SetData(args.ExtractEntity, VisualEnum, Value);

        return true;
    }

    public override float GetTimeNeeded()
    {
        return 0f;
    }

    public override bool GetSpendOnUse()
    {
        return true;
    }
}
