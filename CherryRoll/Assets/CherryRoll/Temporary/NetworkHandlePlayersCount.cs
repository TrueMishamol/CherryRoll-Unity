using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkHandlePlayersCount : NetworkBehaviour {


    public static NetworkHandlePlayersCount Instance { get; private set; }

    public static event EventHandler OnPlayersCountUpdated;

    private NetworkVariable<int> playersCount = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
    }

    private void Player_OnAnyPlayerSpawned(object sender, EventArgs e) {
        Debug.Log("Player_OnAnyPlayerSpawned in NetworkHandleConnection");
        UpdatePlayersCount();
    }

    private void Update() {
        UpdatePlayersCount();
    }

    private void UpdatePlayersCount() {
        // Player Counting for UI
        Debug.Log("Players: " + playersCount.Value.ToString());

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
