using Content.Shared._EstacaoPirata.Attachables;
using Content.Shared.Clothing;
using Robust.Client.GameObjects;
using Robust.Shared.GameStates;

namespace Content.Client._EstacaoPirata.Attachables;

/// <summary>
/// This handles...
/// </summary>
public sealed class AttachableSystem : SharedAttachableSystem
{
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<AttachableSlotComponent, AppearanceChangeEvent>(OnAppearanceChange);
        SubscribeLocalEvent<AttachableSlotComponent, GetEquipmentVisualsEvent>(OnGetEquipmentVisuals);
        SubscribeLocalEvent<AttachableSlotComponent, EquipmentVisualsUpdatedEvent>(OnEquipmentVisualsUpdated);
    }

    private void OnEquipmentVisualsUpdated(EntityUid uid, AttachableSlotComponent component, ref EquipmentVisualsUpdatedEvent args)
    {
        Log.Debug($"OnEquipmentVisualsUpdated");
    }

    private void OnGetEquipmentVisuals(EntityUid uid, AttachableSlotComponent component, ref GetEquipmentVisualsEvent args)
    {
        Log.Debug($"OnGetEquipmentVisuals");

        //args.Layers.;
    }

    private void OnAppearanceChange(EntityUid uid, AttachableSlotComponent component, ref AppearanceChangeEvent args)
    {
        Log.Debug($"OnAppearanceChange");

        if (args.Sprite == null)
            return;

        if (!args.AppearanceData.TryGetValue(AttachableVisuals.VisualState, out var visualStateObject))
            return;

        // if (!args.AppearanceData.TryGetValue(AttachableVisuals.VisualState, out var visualStateObject) ||
        //     visualStateObject is not VendingMachineVisualState visualState)
        // {
        //     visualState = VendingMachineVisualState.Normal;
        // }

        if (!args.AppearanceData.TryGetValue(AttachableVisuals.VisualState, out var value))
        {
            return;
        }

        var layer = (AttachableVisualLayers) value;

        // Colocar um codigo aqui pra saber o que fazer com o sprite antes de atualizar
        UpdateAppearance(uid, layer, component, args.Sprite);
    }

    private void UpdateAppearance(EntityUid uid, AttachableVisualLayers visualLayer, AttachableSlotComponent component, SpriteComponent sprite)
    {
        //SetLayerState(AttachableVisualLayers.Base, component.OffState, sprite);

        // Preciso deixar isto mais complexo
        sprite.LayerSetVisible(visualLayer, true);

        sprite.LayerGetState(1); //retorno: icon-light

        sprite.LayerMapGet(visualLayer); //retorno: 1

        sprite.LayerGetState(sprite.LayerMapGet(visualLayer)); //retorno: icon-light

        // Tem isto aqui tbm de RSI state
        //var rsi =sprite.LayerGetActualRSI(visualLayer);
        //rsi.AddState();

        // Talvez fazer algo com addlayer para adicionar uma layer nova para cada attachment??
        //sprite.AddLayer()
    }
}
