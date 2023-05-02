using Unity.Netcode;
using UnityEngine;

public class Counter : NetworkBehaviour, IInteractableObject, IItemParent {

    [SerializeField] private ItemSO itemSO;
    [SerializeField] private Transform itemHolder;

    private Item item;

    public void Interact(Player player) {
        if (!HasItem()) {
            // There is no Item on Counter
            Item.SpawnItem(itemSO, this);
        } else {
            // There is Item on Counter
            if (!player.HasItem()) {
                // Player is emptyhanded
                // Give Item to the Player
                item.SetItemParent(player);
            } else {
                // Player holds Item
            }
        }
    }

    public Transform GetItemFollowTransform() {
        return itemHolder;
    }

    public void SetItem(Item item) {
        this.item = item;
    }

    public Item GetItem() {
        return item;
    }

    public void ClearItem() {
        item = null;
    }

    public bool HasItem() {
        return item != null;
    }

    public NetworkObject GetNetworkObject() {
        return NetworkObject;
    }
}
