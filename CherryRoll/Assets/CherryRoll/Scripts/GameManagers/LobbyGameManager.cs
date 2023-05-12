using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class LobbyGameManager : NetworkBehaviour {


    //public static LobbyGameManager Instance { get; private set; }

    [SerializeField] private Transform playerPrefab;


    public override void OnNetworkSpawn() {
        Debug.Log("OnNetworkSpawn");

        if (IsServer) {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }

        if (!IsServer) {
            ulong clientId = NetworkManager.LocalClientId;
            InstantiatePlayerPrefabServerRpc(clientId);
            Debug.Log("SceneManager_OnLoadEventCompleted - OnNetworkSpawn");
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut) {
        ulong clientId = NetworkManager.LocalClientId;

        InstantiatePlayerPrefabServerRpc(clientId);
        Debug.Log("SceneManager_OnLoadEventCompleted");
    }

    [ServerRpc (RequireOwnership = false)]
    private void InstantiatePlayerPrefabServerRpc(ulong clientId) {
        Transform playerTransform = Instantiate(playerPrefab);
        playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        Debug.Log("InstantiatePlayerPrefabServerRpc, Client ID: " + clientId);

    }
}
