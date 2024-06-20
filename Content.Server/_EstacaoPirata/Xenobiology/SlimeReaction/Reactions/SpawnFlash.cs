using Content.Server.Flash;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Shared.Flash.Components;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeReaction.Reactions;

public sealed partial class SpawnFlash : SlimeReagentEffect
{
    [DataField("range")]
    public float Range = 1.0f;

    [DataField("duration")]
    public float Duration = 8.0f;

    public override bool Effect(SlimeReagentEffectArgs args)
    {
        var flash = args.EntityManager.System<FlashSystem>();

        var extract = args.ExtractEntity;

        var flashComponent = args.EntityManager.EnsureComponent<FlashComponent>(extract);

        var entity = new Entity<FlashComponent?>
        {
            Owner = extract,
            Comp = flashComponent
        };

        flash.FlashArea(source: entity, user: entity, range: Range, duration: Duration * 1000f);

        var transform = args.EntityManager.System<SharedTransformSystem>();

        args.EntityManager.Spawn("GrenadeFlashEffect", transform.GetMapCoordinates(extract));

        return true;
    }

    public override float TimeNeeded()
    {
        return 5f;
    }
}
