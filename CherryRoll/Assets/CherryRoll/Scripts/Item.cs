using Unity.Netcode;
using UnityEngine;

public class Item : NetworkBehaviour {

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

        //transform.parent = itemParent.GetItemFollowTransform();
        //transform.localPosition = Vector3.zero;


        //Transform itemHolder = itemParent.GetItemFollowTransform();
        //this. itemHolder
    }

    public IItemParent GetItemParent() {
        return itemParent;
    }

    public static void SpawnItem(ItemSO itemSO, IItemParent itemParent) {
        Multiplayer.Instance.SpawnItem(itemSO, itemParent);
    }
}
