<<<<<<< HEAD
using Content.Shared.Explosion;
using Content.Shared.Inventory;
=======
using Content.Server.Storage.EntitySystems;
using Content.Shared.Explosion;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Content.Shared.Storage;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

namespace Content.Server.Inventory
{
    public sealed class ServerInventorySystem : InventorySystem
    {
        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<InventoryComponent, BeforeExplodeEvent>(OnExploded);
<<<<<<< HEAD
=======
            SubscribeNetworkEvent<OpenSlotStorageNetworkMessage>(OnOpenSlotStorage);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
        }

        private void OnExploded(Entity<InventoryComponent> ent, ref BeforeExplodeEvent args)
        {
            // explode each item in their inventory too
            var slots = new InventorySlotEnumerator(ent);
            while (slots.MoveNext(out var slot))
<<<<<<< HEAD
=======
            {
                if (slot.ContainedEntity != null)
                    args.Contents.Add(slot.ContainedEntity.Value);
            }
        }

        private void OnOpenSlotStorage(OpenSlotStorageNetworkMessage ev, EntitySessionEventArgs args)
        {
            if (args.SenderSession.AttachedEntity is not { Valid: true } uid)
                    return;

            if (TryGetSlotEntity(uid, ev.Slot, out var entityUid) && TryComp<StorageComponent>(entityUid, out var storageComponent))
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            {
                if (slot.ContainedEntity != null)
                    args.Contents.Add(slot.ContainedEntity.Value);
            }
        }

        public void TransferEntityInventories(Entity<InventoryComponent?> source, Entity<InventoryComponent?> target)
        {
            if (!Resolve(source.Owner, ref source.Comp) || !Resolve(target.Owner, ref target.Comp))
                return;

            var enumerator = new InventorySlotEnumerator(source.Comp);
            while (enumerator.NextItem(out var item, out var slot))
            {
                if (TryUnequip(source, slot.Name, true, true, inventory: source.Comp))
                    TryEquip(target, item, slot.Name , true, true, inventory: target.Comp);
            }
        }
    }
}
