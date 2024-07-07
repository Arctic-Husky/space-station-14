using System.Text;
using Content.Server._EstacaoPirata.Xenobiology.ExtractScanner.Components;
using Content.Server._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Research.Systems;
using Content.Shared._EstacaoPirata.Xenobiology.ExtractScanner;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeReaction;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Popups;
using Content.Shared.Research.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Utility;

namespace Content.Server._EstacaoPirata.Xenobiology.ExtractScanner;

/// <summary>
/// This handles...
/// </summary>
public sealed class ExtractScannerSystem : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _ui = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlotsSystem = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly PowerReceiverSystem _power = default!;
    [Dependency] private readonly SlimeReactionSystem _slimeReaction = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly ResearchSystem _research = default!;

    public override void Initialize()
    {
        // UI & Sprite Client -> Server
        SubscribeLocalEvent<ExtractScannerComponent, ExtractScannerServerSelectionMessage>(OnServerSelection);
        SubscribeLocalEvent<ExtractScannerComponent, ExtractSellMessage>(OnSell);
        SubscribeLocalEvent<ExtractScannerComponent, ExtractScannerEjectMessage>(OnEject);

        // Power
        SubscribeLocalEvent<ExtractScannerComponent, PowerChangedEvent>(OnPowerChanged);

        // UI & Sprite Server -> Client
        SubscribeLocalEvent<ExtractScannerComponent, EntInsertedIntoContainerMessage>(OnInsertedIntoContainer);
        SubscribeLocalEvent<ExtractScannerComponent, EntRemovedFromContainerMessage>(OnRemovedFromContainer);
        SubscribeLocalEvent<ExtractScannerComponent, ResearchClientServerSelectedMessage>(SubscribeUpdateUiState);
        SubscribeLocalEvent<ExtractScannerComponent, ResearchClientServerDeselectedMessage>(SubscribeUpdateUiState);
        SubscribeLocalEvent<ExtractScannerComponent, ResearchClientSyncMessage>(SubscribeUpdateUiState);
    }

    private void OnRemovedFromContainer(Entity<ExtractScannerComponent> ent, ref EntRemovedFromContainerMessage args)
    {
        _appearance.SetData(ent, ExtractScannerVisualState.Scanning, false);
        UpdateUserInterface(args.Container.Owner, ent.Comp);
    }

    private void OnInsertedIntoContainer(Entity<ExtractScannerComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        if(_power.IsPowered(ent))
            _appearance.SetData(ent, ExtractScannerVisualState.Scanning, true);

        UpdateUserInterface(args.Container.Owner, ent.Comp);
    }

    private void OnEject(EntityUid uid, ExtractScannerComponent component, ExtractScannerEjectMessage args)
    {
        var item = _itemSlotsSystem.GetItemOrNull(uid, "extract_slot");

        _itemSlotsSystem.TryGetSlot(uid, "extract_slot", out var itemSlot);

        if (item is not null && itemSlot is not null)
        {
            _itemSlotsSystem.TryEjectToHands(item.Value, itemSlot, args.Actor);
        }
    }

    private void OnSell(EntityUid uid, ExtractScannerComponent component, ExtractSellMessage args)
    {
        var item = _itemSlotsSystem.GetItemOrNull(uid, "extract_slot");

        if (item is null)
            return;

        var value = GetSellValue(item.Value);

        if (!_research.TryGetClientServer(uid, out var server, out var serverComponent))
            return;

        _research.ModifyServerPoints(server.Value, value, serverComponent);

        _audio.PlayPvs(component.SellSound, uid);

        // Popup entity

        QueueDel(item);

        UpdateUserInterface(uid);
    }

    private void OnServerSelection(EntityUid uid, ExtractScannerComponent component, ExtractScannerServerSelectionMessage args)
    {
        _ui.OpenUi(uid, ResearchClientUiKey.Key, args.Actor);
    }

    private void OnPowerChanged(EntityUid uid, ExtractScannerComponent component, PowerChangedEvent args)
    {
        if (!args.Powered)
        {
            _appearance.SetData(uid, ExtractScannerVisualState.Scanning, false);
        }

        if(args.Powered)
        {
            var item = _itemSlotsSystem.GetItemOrNull(uid, "extract_slot");

            if (item is not null)
            {
                _appearance.SetData(uid, ExtractScannerVisualState.Scanning, true);
            }
            else
            {
                _appearance.SetData(uid, ExtractScannerVisualState.Scanning, false);
            }
        }
    }

    private void SubscribeUpdateUiState<T>(Entity<ExtractScannerComponent> ent, ref T ev)
    {
        UpdateUserInterface(ent);
    }

    private void UpdateUserInterface(EntityUid scanner, ExtractScannerComponent? component = null)
    {
        if (!Resolve(scanner, ref component, false))
            return;

        NetEntity? containedSolid = null;
        FormattedMessage? extractInfo = null;
        bool serverConnected = false;
        FormattedMessage? value = null;

        var item = _itemSlotsSystem.GetItemOrNull(scanner, "extract_slot");
        containedSolid = GetNetEntity(item);

        // Text
        if (containedSolid.HasValue)
        {
            if (item is not null)
            {
                extractInfo = new FormattedMessage();
                extractInfo.AddMarkup(Loc.GetString("extract-scanner-item-name", ("item", item)));
                extractInfo.PushNewline();

                List<string> reagentList = new();
                foreach (var reagent in _slimeReaction.GetReactionsList(item.Value))
                {
                    reagentList.Add(Loc.GetString("extract-scanner-item-reactions-wrapper", ("reaction", Loc.GetString(reagent))));
                }

                var text2 = new StringBuilder();
                text2.Append("\n");
                foreach (var reagent in reagentList)
                {
                    text2.Append("\t\t\t\t\t\t" + reagent);
                    text2.Append("\n");
                }
                var formattedString = Loc.GetString("extract-scanner-item-reactions", ("reactions", text2));
                extractInfo.AddMarkup(Loc.GetString(formattedString));

                extractInfo.PushNewline();

                value = new FormattedMessage();
                value.AddMarkup(Loc.GetString("extract-scanner-item-value", ("value", GetSellValue(item.Value))));
            }
        }

        serverConnected = TryComp<ResearchClientComponent>(scanner, out var client) && client.ConnectedToServer;

        var state = new ExtractScannerUpdateState(containedSolid, extractInfo, serverConnected, value);

        _ui.SetUiState(scanner, ExtractScannerUiKey.Key, state);
    }

    public int GetSellValue(EntityUid uid)
    {
        if (!TryComp<ExtractValueComponent>(uid, out var extractValueComponent))
            return 0;

        float value = extractValueComponent.BaseValue * extractValueComponent.Tier;

        if (value == 0)
            return (int)MathF.Round(value);

        if (TryComp<SlimeReactionComponent>(uid, out var slimeReactionComponent))
        {
            if(slimeReactionComponent.Spent)
            {
                value /= 2;
            }
        }

        return (int)MathF.Round(value);
    }
}
