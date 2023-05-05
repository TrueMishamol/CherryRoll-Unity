using Unity.Netcode;

using UnityEngine;

public class FollowTransform : NetworkBehaviour {


    private Transform targetTransform;

    //[SerializeField] private NetworkVariable<Transform> networkVariableTransform = new NetworkVariable<Transform>(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    private void Awake() {
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    }

    private void LateUpdate() {
        if (targetTransform == null) {
            return;
        }

        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }

    public void SetTargetTransform(Transform targetTransform) {
        this.targetTransform = targetTransform;
    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj) {
        RefreshTargetTransform();
    }

    private void RefreshTargetTransform() {
        if (!IsServer) return;

        //transformNetworkObjectReference = targetTransform;
        IItemParent targetTransformItemParent = targetTransform.GetComponent<IItemParent>();

        RefreshTargetTransformClientRpc(targetTransformItemParent.GetNetworkObject());
        //NetworkVariable
    }

    //[ServerRpc(RequireOwnership = false)]
    //private void RefreshTargetTransformServerRpc(NetworkVariable<Transform> networkVariableTransform) {
    //    // Сервер будет хранить данные о targetTransform и при подключении нового игрока, все игроки берут и обновляют эти данные у себя в соответствии с сервером. Либо только подключившийся игрок
    //}

    [ClientRpc]
    private void RefreshTargetTransformClientRpc(NetworkObjectReference transformNetworkObjectReference) {
        transformNetworkObjectReference.TryGet(out NetworkObject transformNetworkObject);
        Transform transform = transformNetworkObject.transform;

        targetTransform = transform;
    }

    //NetworkObjectReference
}
