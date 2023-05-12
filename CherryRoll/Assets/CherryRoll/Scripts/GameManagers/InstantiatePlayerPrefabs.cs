using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class InstantiatePlayerPrefabs : NetworkBehaviour {
    

    //public static CollectThePlateGameManager Instance { get; private set; }

    [SerializeField] private Transform playerPrefab;


    public override void OnNetworkSpawn() {
        if (IsServer) {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut) {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }
}
