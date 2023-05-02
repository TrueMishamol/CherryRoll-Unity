using Unity.Netcode;
using UnityEngine;

public class CounterInteractableObject : BaseInteractableObject, IItemParent {

    [SerializeField] private ItemSO itemSO;
    //[SerializeField] private Transform flourPrefab;
    [SerializeField] private Transform itemHolder;

    private Item item;

    public override void Interact(Player player) {
        if (!HasItem()) {
            Item.SpawnItem(itemSO, player);
            //!Item.SpawnItem(itemSO, this);
        } else {
            // Give Item to the Player
            item.SetItemParent(player);
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
        return null;
    }
}
