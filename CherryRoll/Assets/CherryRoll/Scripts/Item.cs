using Unity.Netcode;
using UnityEngine;

public class Item : NetworkBehaviour {


    [SerializeField] private ItemSO itemSO;

    private IItemParent itemParent;
    private FollowTransform followTransform;


    private void Awake() {
        followTransform = GetComponent<FollowTransform>();
    }

    public ItemSO GetItemSO() {
        return itemSO;
    }

    public void SetItemParent(IItemParent itemParent) {
        SetItemParentServerRpc(itemParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetItemParentServerRpc(NetworkObjectReference itemParentNetworkObjectReference) {
        SetItemParentClientRpc(itemParentNetworkObjectReference);
    }

    [ClientRpc]
    private void SetItemParentClientRpc(NetworkObjectReference itemParentNetworkObjectReference) {
        itemParentNetworkObjectReference.TryGet(out NetworkObject itemParentNetworkObject);
        IItemParent itemParent = itemParentNetworkObject.GetComponent<IItemParent>();

        //Debug.Log("Cleat Item");

        if (this.itemParent != null) {
            //^ Clear Current Item Parent (to re-Parent)
            this.itemParent.ClearItem();
        }

        this.itemParent = itemParent;

        if (itemParent.HasItem()) {
            Debug.LogError("IItemParent already has Item");
        }

        itemParent.SetItem(this);

        followTransform.SetTargetTransform(itemParent);
    }

    public IItemParent GetItemParent() {
        return itemParent;
    }

    public static void SpawnItem(ItemSO itemSO, IItemParent itemParent) {
        Multiplayer.Instance.SpawnItem(itemSO, itemParent);
    }

    public NetworkObject GetNetworkObject() {
        return NetworkObject;
    }

    public void DestroySelf() {
        Destroy(gameObject);      
    }

    public void ClearItemOnParent() {
        itemParent.ClearItem();
    }

    public static void DestroyItem(Item item) {
        Multiplayer.Instance.DestroyItem(item);
    }


    public void RefreshItemParent() {
        if (!IsServer) return;

        RefreshItemParentClientRpc(itemParent.GetNetworkObject());
    }

    [ClientRpc]
    private void RefreshItemParentClientRpc(NetworkObjectReference itemParentNetworkObjectReference) {
        itemParentNetworkObjectReference.TryGet(out NetworkObject itemParentNetworkObject);
        itemParent = itemParentNetworkObject.GetComponent<IItemParent>();
    }

    public int GetItemCost() {
        return itemSO.itemCost;
    }
}
