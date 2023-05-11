using System;
using Unity.Netcode;
using UnityEngine;

public class MultiplayerPlayersCount : NetworkBehaviour {


    public static MultiplayerPlayersCount Instance { get; private set; }

    public static event EventHandler OnPlayersCountUpdated;

    private int maxPlayerAmount = 10;

    private NetworkVariable<int> playersCount = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
    }

    private void Player_OnAnyPlayerSpawned(object sender, EventArgs e) {
        UpdatePlayersCount();
    }

    //! Refactor
    private void Update() {
        UpdatePlayersCount();
    }

    private void UpdatePlayersCount() {
        //Debug.Log("Players: " + playersCount.Value.ToString());

        if (!IsServer) return;
        try {
            playersCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
        } catch (NotServerException) {
            // If the host stops, then constantly occurs thiss exception Unity.Netcode.NotServerException: ConnectedClients should only be accessed on server
            Debug.Log("Server stopped");
            return;
        }

        OnPlayersCountUpdated?.Invoke(this, EventArgs.Empty);
    }

    public int GetPlayersCount() {
        return playersCount.Value;
    }
}
