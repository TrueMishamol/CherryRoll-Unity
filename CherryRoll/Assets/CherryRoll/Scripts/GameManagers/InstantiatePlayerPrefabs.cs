using Unity.Netcode;
using UnityEngine;


public class InstantiatePlayerPrefabs : NetworkBehaviour {


    [SerializeField] private Transform playerPrefab;

    [SerializeField] float spawnRadius = 5f;


    public override void OnNetworkSpawn() {
        ulong clientId = NetworkManager.LocalClientId;
        InstantiatePlayerPrefabServerRpc(clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void InstantiatePlayerPrefabServerRpc(ulong clientId) {
        Transform playerTransform = Instantiate(playerPrefab);
        playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

        //RandomSpawnRadiusServerRpc(playerTransform.GetComponent<Player>().NetworkObject);
    }

    //! This code is not working
    //[ServerRpc(RequireOwnership = false)]
    //private void RandomSpawnRadiusServerRpc(NetworkObjectReference playerNetworkObjectReference) {
    //    Debug.Log("RandomSpawnRadiusServerRpc");
    //    playerNetworkObjectReference.TryGet(out NetworkObject playerNetworkObject);
    //    Debug.Log(playerNetworkObject);

    //    playerNetworkObject.transform.position = new Vector3(Random.Range(spawnRadius, -spawnRadius), 0, Random.Range(spawnRadius, -spawnRadius));

    //    //// Rotates player to face Camera
    //    //transform.rotation = new Quaternion(0, 180, 0, 0);
    //}
}
