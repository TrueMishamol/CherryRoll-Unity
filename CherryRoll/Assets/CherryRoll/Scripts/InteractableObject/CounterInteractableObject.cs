using UnityEngine;

public class CounterInteractableObject : BaseInteractableObject, IItemParent {

    [SerializeField] private ItemSO itemSO;
    //[SerializeField] private Transform flourPrefab;
    [SerializeField] private Transform itemHolder;

    private Item item;

    public override void Interact(Player player) {
        if (item == null) {
            SpawnItem();
        } else {
            // Give Item to the Player
            item.SetItemParent(player);
        }
    }

    private void SpawnItem() {
        Transform itemTransform = Instantiate(itemSO.prefab, itemHolder);
        itemTransform.GetComponent<Item>().SetItemParent(this);
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
}
