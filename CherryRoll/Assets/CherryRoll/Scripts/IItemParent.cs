using UnityEngine;

public interface IItemParent {

    public Transform GetItemFollowTransform();

    public void SetItem(Item item);

    public Item GetItem();

    public void ClearItem();

    public bool HasItem();
}
