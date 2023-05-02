using Unity.Netcode;
using UnityEngine;

public class Multiplayer : NetworkBehaviour {

    public static Multiplayer Instance { get; private set; }

    [SerializeField] private ItemSOListSO itemSOListSO;

    private void Awake() {
        Instance = this;
    }

    public void SpawnItem(ItemSO itemSO, IItemParent itemParent) {
        SpawnItemServerRpc(GetItemSOIndex(itemSO), itemParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnItemServerRpc(int itemSOIndex, NetworkObjectReference itemParentNetworkObjectReference) {
        ItemSO itemSO = GetItemSO(itemSOIndex);

        Transform itemTransform = Instantiate(itemSO.prefab);

        NetworkObject itemNetworkObject = itemTransform.GetComponent<NetworkObject>();
        itemNetworkObject.Spawn(true);

        Item item = itemTransform.GetComponent<Item>();

        itemParentNetworkObjectReference.TryGet(out NetworkObject itemParentNetworkObject);
        IItemParent itemParent = itemParentNetworkObject.GetComponent<IItemParent>();

        item.SetItemParent(itemParent);
    }

    private int GetItemSOIndex(ItemSO itemSO) {
        return itemSOListSO.itemSOList.IndexOf(itemSO);
    }

    private ItemSO GetItemSO(int itemSOIndex) {
        return itemSOListSO.itemSOList[itemSOIndex];
    }
}
