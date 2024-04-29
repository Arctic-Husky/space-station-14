using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;

namespace Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction.Reactions;

public sealed partial class FillExtract : SlimeReagentEffect
{
    // TODO: mudar isto para uma lista de reagentes e suas quantidades, um dicionario
    [DataField("reagent")]
    public string Reagent;

    // TODO: lidar com esta quantidade minima
    [DataField("minReagentRequired")]
    public float MinReagentRequired;

    // Multiplicar a quantidade de, por exemplo, plasma inserido por este numero, essa vai ser a quantidade produzida, com limite ate o maximo do extract
    [DataField("rate")]
    public float Rate = 1f;

    // Se isto aqui for marcado como verdadeiro, burlar o codigo de rate e encher ate o bico
    [DataField("fillToMax")]
    public bool FillToMax = false;

    public override bool Effect(SlimeReagentEffectArgs args)
    {
        if (!FillSolution(args,FillToMax,Rate))
            return false;

        var audioSystem = args.EntityManager.System<SharedAudioSystem>();

        PlaySound(audioSystem, args.ReactionComponent.ReactionSound, args.ExtractEntity);

        return true;
    }

    private bool FillSolution(SlimeReagentEffectArgs args, bool fillToMax = false, float rate = 1f)
    {
        var extractEntity = args.ExtractEntity;

        var solutionContainerSystem = args.EntityManager.System<SharedSolutionContainerSystem>();
        var solutionContainerManagerComponent = args.EntityManager.GetComponent<SolutionContainerManagerComponent>(extractEntity);
        var slimeReactionComponent = args.EntityManager.GetComponent<SlimeReactionComponent>(extractEntity);

        var entity = new Entity<SolutionContainerManagerComponent?>(extractEntity, solutionContainerManagerComponent);

        if (!solutionContainerSystem.TryGetSolution(entity, slimeReactionComponent.SolutionName, out var soln))
            return false;

        if (FillToMax)
        {
            soln.Value.Comp.Solution.AddReagent(Reagent, soln.Value.Comp.Solution.MaxVolume);
        }
        else
        {
            var amount = args.Quantity * rate;
            if (amount > soln.Value.Comp.Solution.MaxVolume)
            {
                amount = soln.Value.Comp.Solution.MaxVolume;
            }
            // Talvez lidar com minimo ou rate de a cada 1u inteiro adicionar, nao menos

            soln.Value.Comp.Solution.AddReagent(Reagent, amount);
        }

        return true;
    }

    public override float NeedsTime()
    {
        return 0;
    }

    public override void PlaySound(SharedAudioSystem audioSystem, SoundSpecifier? sound, EntityUid entity)
    {
        audioSystem.PlayPvs(sound, entity);
    }
}
