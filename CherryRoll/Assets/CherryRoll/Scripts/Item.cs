using UnityEngine;

public class Item : MonoBehaviour {

    [SerializeField] private ItemSO itemSO;

    private IItemParent itemParent;

    public ItemSO GetItemSO() {
        return itemSO;
    }

    public void SetItemParent(IItemParent itemParent) {
        if (this.itemParent != null) {
            this.itemParent.ClearItem();
        }

        this.itemParent = itemParent;

        if (itemParent.HasItem()) {
            Debug.LogError("IItemParent already has Item");
        }

        itemParent.SetItem(this);

        transform.parent = itemParent.GetItemFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IItemParent GetItemParent() {
        return itemParent;
    }
}
