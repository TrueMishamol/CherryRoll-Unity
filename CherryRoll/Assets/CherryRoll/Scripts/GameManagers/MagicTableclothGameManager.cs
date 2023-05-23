using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MagicTableclothGameManager : NetworkBehaviour {


    public event EventHandler OnRequestCompleted;
    public event EventHandler OnRequestSuccess;
    public event EventHandler OnRequestFailed;


    public static MagicTableclothGameManager Instance { get; private set; }

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

        Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
    }

    private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e) {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            if (!playersScoresDictionary.ContainsKey(clientId)) {
                playersScoresDictionary[clientId] = 0;
            }
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
