using Unity.Netcode;
using UnityEngine;

public class FollowTransform : NetworkBehaviour {


    private IItemParent targetIItemParent;


    private void Awake() {
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    }

    private void LateUpdate() {
        if (targetIItemParent == null) {
            return;
        }

        Transform targetTransform = targetIItemParent.GetItemFollowTransform();

        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }

    public void SetTargetTransform(IItemParent targetIItemParent) {
        this.targetIItemParent = targetIItemParent;
    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj) {
        RefreshTargetTransform();
        //! Null
        if (targetIItemParent != null) {
            targetIItemParent.RefreshItem();
        }
    }

    private void RefreshTargetTransform() {
        if (!IsServer) return;

        RefreshTargetTransformClientRpc(targetIItemParent.GetNetworkObject());
    }

    [ClientRpc]
    private void RefreshTargetTransformClientRpc(NetworkObjectReference targetNetworkObjectReference) {
        targetNetworkObjectReference.TryGet(out NetworkObject targetNetworkObject);
        targetIItemParent = targetNetworkObject.GetComponent<IItemParent>();
    }
}
