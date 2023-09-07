using System;
using Unity.Netcode;
using UnityEngine;

public class MultiplayerPlayersCount : NetworkBehaviour {


    public static MultiplayerPlayersCount Instance { get; private set; }

    private NetworkVariable<int> playersCount = new NetworkVariable<int>(0);

    public static event EventHandler OnPlayerCountUpdate;

    public static int maxPlayerAmount = 10;

    private bool autoUpdatePlayersCount;


    private void Awake() {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn() {
        Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
        playersCount.OnValueChanged += PlayersCount_OnValueChanged;

        if (IsServer) {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }
    }

    private void LateUpdate() {
        if (!IsServer) return;

        if (autoUpdatePlayersCount) {
            autoUpdatePlayersCount = false;
            UpdatePlayersCount();
        }
    }

    private void PlayersCount_OnValueChanged(int previousValue, int newValue) {
        UpdatePlayersCount();

        OnPlayerCountUpdate?.Invoke(this, EventArgs.Empty);
    }


    private void NetworkManager_OnClientDisconnectCallback(ulong clientId) {
        autoUpdatePlayersCount = true;
        UpdatePlayersCount();
    }

    private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e) {
        autoUpdatePlayersCount = true;
        UpdatePlayersCount();
    }


    public int GetPlayersCount() {
        return playersCount.Value;
    }

    private void UpdatePlayersCount() {
        UpdatePlayerCountServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePlayerCountServerRpc() {
        try {
            playersCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
        } catch (NotServerException) {
            //^ If the host stops, then constantly occurs thiss exception Unity.Netcode.NotServerException: ConnectedClients should only be accessed on server
            Debug.Log("Server stopped");
            playersCount.Value = -1;
        }
    }
}
