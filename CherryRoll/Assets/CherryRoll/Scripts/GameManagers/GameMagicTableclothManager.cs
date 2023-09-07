using System;
using System.Collections.Generic;
using Unity.Netcode;

public class GameMagicTableclothManager : NetworkBehaviour {


    public static GameMagicTableclothManager Instance { get; private set; }

    public event EventHandler OnItemDelivered;
    public event EventHandler OnPlayersScoresDictionaryUpdated;

    private Dictionary<ulong, int> allPlayersScoresDictionary = new Dictionary<ulong, int>(); //^ Server side only
    public Dictionary<ulong, int> connectedPlayersScoresDictionary = new Dictionary<ulong, int>();


    private void Awake() {
        Instance = this;

        if (!IsServer) return;

        allPlayersScoresDictionary = new Dictionary<ulong, int>();
    }

    private void Start() {
        if (!IsServer) return;

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            allPlayersScoresDictionary[clientId] = 0;
        }

        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;

        //^ Присваивание значения 0 для всех клиентов, уже подключенных на сервере
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            allPlayersScoresDictionary[clientId] = 0;
        }

        RecreatePlayersScoresDictionaryServerRpc();
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId) {
        RemovePlayerFromDictionaryClientRpc(clientId);

        OnPlayersScoresDictionaryUpdatedClientRpc();
    }

    [ClientRpc]
    private void RemovePlayerFromDictionaryClientRpc(ulong clientId) {
        connectedPlayersScoresDictionary.Remove(clientId);
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId) {
        //^ Добавление новой строки в словарь с новым подключенным клиентом
        allPlayersScoresDictionary[clientId] = 0;

        RecreatePlayersScoresDictionaryServerRpc();
    }

    private void Player_OnAnyPlayerSpawned(object sender, EventArgs e) {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            if (!allPlayersScoresDictionary.ContainsKey(clientId)) {
                allPlayersScoresDictionary[clientId] = 0;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeliverItemScoreServerRpc(int itemCost = 1, ServerRpcParams serverRpcParams = default) {
        allPlayersScoresDictionary[serverRpcParams.Receive.SenderClientId] += itemCost;
        RecreatePlayersScoresDictionaryServerRpc();

        OnItemDeliveredClientRpc();
    }

    [ClientRpc]
    private void OnItemDeliveredClientRpc() {
        OnItemDelivered?.Invoke(this, EventArgs.Empty);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RecreatePlayersScoresDictionaryServerRpc() {
        CreatePlayersScoresDictionaryClientRpc();

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            UpdatePlayerScoreInDictionaryClientRpc(clientId, allPlayersScoresDictionary[clientId]);
        }

        OnPlayersScoresDictionaryUpdatedClientRpc();
    }

    [ClientRpc]
    private void CreatePlayersScoresDictionaryClientRpc() {
        connectedPlayersScoresDictionary = new Dictionary<ulong, int>();
    }

    [ClientRpc]
    private void UpdatePlayerScoreInDictionaryClientRpc(ulong clientId, int score) {
        connectedPlayersScoresDictionary[clientId] = score;
    }

    [ClientRpc]
    private void OnPlayersScoresDictionaryUpdatedClientRpc() {
        OnPlayersScoresDictionaryUpdated?.Invoke(this, EventArgs.Empty);
    }

    public override void OnDestroy() {
        NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        Player.OnAnyPlayerSpawned -= Player_OnAnyPlayerSpawned;
    }
}
