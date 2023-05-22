using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseGameManager : NetworkBehaviour {


    public static BaseGameManager Instance { get; private set; }

    private Dictionary<ulong, int> playersScoresDictionary;


    private void Awake() {
        Instance = this;

        playersScoresDictionary = new Dictionary<ulong, int>();
    }

    private void Start() {
        if (!IsServer) return;

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            playersScoresDictionary[clientId] = 0;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void IncreasePlayerScoreByOneServerRpc(ServerRpcParams serverRpcParams = default) {
        playersScoresDictionary[serverRpcParams.Receive.SenderClientId]++;
    }

    public virtual void GetPlayersScores() {
        GetPlayersScoresServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void GetPlayersScoresServerRpc() {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            Debug.Log("Player " + clientId + " score is " + playersScoresDictionary[clientId]);
        }

        //GetPlayersScoresClientRpc();
    }

    //[ClientRpc]
    //private void GetPlayersScoresClientRpc() {

    //}
}
