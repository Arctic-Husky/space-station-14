using Content.Server._EstacaoPirata.Xenobiology.ExtractScanner.Components;
using Content.Server.Power.Components;
using Content.Shared._EstacaoPirata.Xenobiology.ExtractScanner;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Shared.Interaction;
using Content.Shared.Item;
using Content.Shared.Popups;
using Content.Shared.Research.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Containers;

namespace Content.Server._EstacaoPirata.Xenobiology.ExtractScanner;

/// <summary>
/// This handles...
/// </summary>
public sealed class ExtractScannerSystem : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _ui = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedItemSystem _item = default!;

    public override void Initialize()
    {
        // UI
        SubscribeLocalEvent<ExtractScannerComponent, ExtractScannerServerSelectionMessage>(OnServerSelection);
        SubscribeLocalEvent<ExtractScannerComponent, ExtractSellMessage>(OnSell);
        SubscribeLocalEvent<ExtractScannerComponent, ExtractScannerEjectMessage>(OnEject);

        // Power
        SubscribeLocalEvent<ExtractScannerComponent, PowerChangedEvent>(OnPowerChanged);

        // Container
        SubscribeLocalEvent<ExtractScannerComponent, InteractUsingEvent>(OnInteract);
        //SubscribeLocalEvent<ExtractScannerComponent, ComponentInit>(OnInit);
    }

    private void OnInteract(Entity<ExtractScannerComponent> ent, ref InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        // TODO colocar uma interacao com verb

        // if (!(TryComp<ApcPowerReceiverComponent>(ent, out var apc) && apc.Powered))
        // {
        //     _popupSystem.PopupEntity(Loc.GetString("extract-scanner-interact-using-no-power"), ent, args.User);
        //     return;
        // }

        if (TryComp<ItemComponent>(args.Used, out var item))
        {
            // check if size of an item you're trying to put in is too big
            if (_item.GetSizePrototype(item.Size) > _item.GetSizePrototype(ent.Comp.MaxItemSize))
            {
                _popupSystem.PopupEntity(Loc.GetString("extract-scanner-interact-item-too-big", ("item", args.Used)), ent, args.User);
                return;
            }

            if (!TryComp<SlimeReactionComponent>(args.Used, out var slimeReactionComponent))
            {
                // enviar mensagem com CanSell false
            }

            // Se tiver um item dentro tirar ele pra colocar o novo no lugar
        }




    }

    private void OnEject(EntityUid uid, ExtractScannerComponent component, ExtractScannerEjectMessage args)
    {

    }

    private void OnSell(EntityUid uid, ExtractScannerComponent component, ExtractSellMessage args)
    {

    }

    private void OnServerSelection(EntityUid uid, ExtractScannerComponent component, ExtractScannerServerSelectionMessage args)
    {
        _ui.OpenUi(uid, ResearchClientUiKey.Key, args.Actor);
    }

    private void OnPowerChanged(EntityUid uid, ExtractScannerComponent component, PowerChangedEvent args)
    {
        if (!args.Powered)
        {

        }
    }
}
