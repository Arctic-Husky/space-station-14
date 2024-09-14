using Content.Client.Items.UI;
using Content.Client.Message;
using Content.Client.Stylesheets;
<<<<<<< HEAD
using Content.Shared.FixedPoint;
using Content.Shared.Tools.Components;
using Content.Shared.Tools.Systems;
using Robust.Client.UserInterface.Controls;
=======
using Content.Client.Tools.Components;
using Content.Shared.Item;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;
using ItemToggleComponent = Content.Shared.Item.ItemToggle.Components.ItemToggleComponent;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

namespace Content.Client.Tools.UI;

public sealed class WelderStatusControl : PollingItemStatusControl<WelderStatusControl.Data>
{
<<<<<<< HEAD
    private readonly Entity<WelderComponent> _parent;
    private readonly IEntityManager _entityManager;
    private readonly SharedToolSystem _toolSystem;
    private readonly RichTextLabel _label;

    public WelderStatusControl(Entity<WelderComponent> parent, IEntityManager entityManager, SharedToolSystem toolSystem)
    {
        _parent = parent;
        _entityManager = entityManager;
        _toolSystem = toolSystem;
=======
    [Dependency] private readonly IEntityManager _entMan = default!;

    private readonly WelderComponent _parent;
    private readonly ItemToggleComponent? _toggleComponent;
    private readonly RichTextLabel _label;

    public WelderStatusControl(Entity<WelderComponent> parent)
    {
        _parent = parent;
        _entMan = IoCManager.Resolve<IEntityManager>();
        if (_entMan.TryGetComponent<ItemToggleComponent>(parent, out var itemToggle))
            _toggleComponent = itemToggle;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        _label = new RichTextLabel { StyleClasses = { StyleNano.StyleClassItemStatus } };
        AddChild(_label);

        UpdateDraw();
    }

    protected override Data PollData()
    {
        var (fuel, capacity) = _toolSystem.GetWelderFuelAndCapacity(_parent, _parent.Comp);
        return new Data(fuel, capacity, _parent.Comp.Enabled);
    }

    protected override void Update(in Data data)
    {
<<<<<<< HEAD
=======
        _parent.UiUpdateNeeded = false;

        var fuelCap = _parent.FuelCapacity;
        var fuel = _parent.Fuel;
        var lit = false;
        if (_toggleComponent != null)
        {
            lit = _toggleComponent.Activated;
        }

>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        _label.SetMarkup(Loc.GetString("welder-component-on-examine-detailed-message",
            ("colorName", data.Fuel < data.FuelCapacity / 4f ? "darkorange" : "orange"),
            ("fuelLeft", data.Fuel),
            ("fuelCapacity", data.FuelCapacity),
            ("status", Loc.GetString(data.Lit ? "welder-component-on-examine-welder-lit-message" : "welder-component-on-examine-welder-not-lit-message"))));
    }

    public record struct Data(FixedPoint2 Fuel, FixedPoint2 FuelCapacity, bool Lit);
}
