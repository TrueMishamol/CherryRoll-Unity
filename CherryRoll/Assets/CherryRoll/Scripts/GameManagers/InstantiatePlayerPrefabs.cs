using Unity.Netcode;
using UnityEngine;


public class InstantiatePlayerPrefabs : NetworkBehaviour {


    [SerializeField] private Transform playerPrefab;


    public override void OnNetworkSpawn() {
        ulong clientId = NetworkManager.LocalClientId;
        InstantiatePlayerPrefabServerRpc(clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void InstantiatePlayerPrefabServerRpc(ulong clientId) {
        Transform playerTransform = Instantiate(playerPrefab);
        playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }
}
