using Content.Shared._EstacaoPirata.Xenobiology.ExtractScanner;
using JetBrains.Annotations;

namespace Content.Client._EstacaoPirata.Xenobiology.UI.ExtractScanner;

[UsedImplicitly]
public sealed class ExtractScannerBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private ExtractScannerMenu? _menu;

    public ExtractScannerBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _menu = new ExtractScannerMenu();
        _menu.OpenCentered();
        _menu.OnClose += Close;


        _menu.ServerSelectionButton.OnPressed += _ => SendPredictedMessage(new ExtractScannerServerSelectionMessage());
        _menu.SellButton.OnPressed += _ => SendPredictedMessage(new ExtractSellMessage());
        _menu.EjectButton.OnPressed += _ => SendPredictedMessage(new ExtractScannerEjectMessage());
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
        {
            return;
        }

        _menu?.Dispose();
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        switch (state)
        {
            case ExtractScannerUpdateState msg:
                _menu?.SetButtonsDisabled(msg);
                _menu?.UpdateInformationDisplay(msg);
                break;
        }
    }
}
