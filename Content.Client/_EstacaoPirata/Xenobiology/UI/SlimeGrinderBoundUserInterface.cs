using Content.Shared._EstacaoPirata.Xenobiology.SlimeGrinder;
using Content.Shared.Chemistry.Reagent;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Timing;

namespace Content.Client._EstacaoPirata.Xenobiology.UI;

[UsedImplicitly]
public sealed class SlimeGrinderBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private SlimeGrinderMenu? _menu;

    [ViewVariables]
    private readonly Dictionary<int, EntityUid> _solids = new();

    // [ViewVariables]
    // private readonly Dictionary<int, ReagentQuantity> _reagents = new(); // sei la

    [Dependency] private readonly IGameTiming _gameTiming = default!;

    public SlimeGrinderUpdateUserInterfaceState currentState = default!;

    private IEntityManager _entManager;

    public SlimeGrinderBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        _entManager = IoCManager.Resolve<IEntityManager>();
    }

    public TimeSpan GetCurrentTime()
    {
        return _gameTiming.CurTime;
    }

    protected override void Open()
    {
        base.Open();

        _menu = new SlimeGrinderMenu(this);
        _menu.OpenCentered();
        _menu.OnClose += Close;
        _menu.StartButton.OnButtonDown += _ => SendPredictedMessage(new SlimeGrinderStartGrindingMessage());
        _menu.StartButton.OnButtonUp += _ => SendPredictedMessage(new SlimeGrinderStopGrindingMessage());
        // _menu.Eject
        _menu.InsertedSlimesList.OnItemSelected += args =>
        {
            SendPredictedMessage(new SlimeGrinderEjectSolidIndexedMessage(EntMan.GetNetEntity(_solids[args.ItemIndex])));
        };
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
        {
            return;
        }

        _solids.Clear();
        _menu?.Dispose();
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not SlimeGrinderUpdateUserInterfaceState cState)
        {
            return;
        }

        currentState = cState;

        // Wizards vai tentar mover isso pra um component state, ficar de olho no bound user interface do micro-ondas
        RefreshContentsDisplay(_entManager.GetEntityArray(cState.ContainedSolids));

        if (_menu == null) return;

        //_menu.StartButton.Disabled = cState.ContainedSolids.Length == 0;

        // //Set the ui color to indicate if the grinder is running or not
        // if (cState.IsBusy && cState.ContainedSolids.Length > 0)
        // {
        //     _menu.InsertedSlimesPanel.PanelOverride = new StyleBoxFlat { BackgroundColor = Color.FromHex("#947300") };
        // }
        // else
        // {
        //     _menu.InsertedSlimesPanel.PanelOverride = new StyleBoxFlat { BackgroundColor = Color.FromHex("#1B1B1E") };
        // }
    }

    private void RefreshContentsDisplay(EntityUid[] containedSolids)
    {
        //_reagents.Clear();

        if (_menu == null) return;

        _solids.Clear();
        _menu.InsertedSlimesList.Clear();

        foreach (var entity in containedSolids)
        {
            if (EntMan.Deleted(entity))
            {
                return;
            }

            // TODO just use sprite view

            Texture? texture;
            if (EntMan.TryGetComponent<IconComponent>(entity, out var iconComponent))
            {
                texture = EntMan.System<SpriteSystem>().GetIcon(iconComponent);
            }
            else if (EntMan.TryGetComponent<SpriteComponent>(entity, out var spriteComponent))
            {
                texture = spriteComponent.Icon?.Default;
            }
            else
            {
                continue;
            }

            var solidItem = _menu.InsertedSlimesList.AddItem(EntMan.GetComponent<MetaDataComponent>(entity).EntityName, texture);
            var solidIndex = _menu.InsertedSlimesList.IndexOf(solidItem);
            _solids.Add(solidIndex, entity);
        }
    }
}
