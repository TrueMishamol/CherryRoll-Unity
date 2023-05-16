using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class InstantiatePlayerPrefabs : NetworkBehaviour {


    [SerializeField] private Transform playerPrefab;


    public override void OnNetworkSpawn() {
        if (IsServer) {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }

        if (!IsServer) {
            ulong clientId = NetworkManager.LocalClientId;
            InstantiatePlayerPrefabServerRpc(clientId);
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut) {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            InstantiatePlayerPrefabServerRpc(clientId);
        }

        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= SceneManager_OnLoadEventCompleted;
    }

    [ServerRpc(RequireOwnership = false)]
    private void InstantiatePlayerPrefabServerRpc(ulong clientId) {
        Transform playerTransform = Instantiate(playerPrefab);
        playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }
}
