using Content.Client.Storage.Systems;
using Content.Shared.Storage;
using JetBrains.Annotations;

namespace Content.Client.Storage;

[UsedImplicitly]
public sealed class StorageBoundUserInterface : BoundUserInterface
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    private readonly StorageSystem _storage;

    public StorageBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        IoCManager.InjectDependencies(this);
        _storage = _entManager.System<StorageSystem>();
    }

<<<<<<< HEAD
    protected override void Open()
    {
        base.Open();

        if (_entManager.TryGetComponent<StorageComponent>(Owner, out var comp))
            _storage.OpenStorageWindow((Owner, comp));
    }

=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
            return;

        _storage.CloseStorageWindow(Owner);
<<<<<<< HEAD
=======
    }

    protected override void ReceiveMessage(BoundUserInterfaceMessage message)
    {
        base.ReceiveMessage(message);

        if (message is StorageModifyWindowMessage)
        {
            if (_entManager.TryGetComponent<StorageComponent>(Owner, out var comp))
                _storage.OpenStorageWindow((Owner, comp));
        }
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }
}

