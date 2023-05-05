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

        if (this.itemParent != null) {
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
}
