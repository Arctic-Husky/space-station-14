using System.Linq;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeReaction.Reactions;

// TODO: problema, component.Used vai fazer com que o slime nao reaja a mais nada
public sealed partial class ChangeAttribute : SlimeReagentEffect
{
    [DataField("comp")]
    public ComponentRegistry Comp = new();

    [DataField("field")]
    public string Field;

    [DataField("type")]
    public string FieldType;

    [DataField("value")]
    public string Value;

    private static readonly Dictionary<string, Type> TypeDictionary = new ()
    {
        {"bool", typeof(bool)},
        {"int", typeof(int)},
        {"float", typeof(float)},
        {"double", typeof(double)},
        // add more if you need
    };

    public override bool Effect(SlimeReagentEffectArgs args)
    {
        // Isto funciona yayyyyyy
        var type = Comp.Values.First().Component.GetType();

        args.EntityManager.TryGetComponent(args.ExtractEntity, type, out var componentFromManager);

        if (componentFromManager == null)
        {
            return false;
        }

        var field = type.GetField(Field);

        TypeDictionary.TryGetValue(FieldType, out var value);

        if (value == null)
        {
            return false;
        }

        var convertedValue = Convert.ChangeType(Value, value);

        if (field != null)
        {
            field.SetValue(componentFromManager, convertedValue);
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

    public override void PlaySound(SharedAudioSystem audioSystem, SoundSpecifier? sound, EntityUid entity)
    {

    }
}


// // Isto funciona yayyyyyy
// var type = Comp.Values.First().Component.GetType();
//
// var componentFromManager = args.EntityManager.GetComponent(args.ExtractEntity, type);
//
//
// var field = type.GetField("teste");
//
// if (field != null)
// {
//     field.SetValue(componentFromManager, 2);
// }
