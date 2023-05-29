using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MagicTableclothGameManager : NetworkBehaviour {


    public static MagicTableclothGameManager Instance { get; private set; }

    public event EventHandler OnItemDelivered;

    private Dictionary<ulong, int> allPlayersScoresDictionary = new Dictionary<ulong, int>(); // Server side only
    public Dictionary<ulong, int> connectedPlayersScoresDictionary = new Dictionary<ulong, int>();


    private void Awake() {
        Instance = this;

        if (!IsServer) return; //! Не работает должным образом и возвращает даже если сервер

        allPlayersScoresDictionary = new Dictionary<ulong, int>();
        Debug.Log("Create new Dictionary");
    }

    private void Start() {
        if (!IsServer) return;

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            allPlayersScoresDictionary[clientId] = 0;
        }

        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;

        // Присваивание значения 0 для всех клиентов, уже подключенных на сервере
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            allPlayersScoresDictionary[clientId] = 0;
        }

        UpdatePlayersScoresDictionaryServerRpc();
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId) {
        // Добавление новой строки в словарь с новым подключенным клиентом
        allPlayersScoresDictionary[clientId] = 0;

        UpdatePlayersScoresDictionaryServerRpc();
    }

    private void Player_OnAnyPlayerSpawned(object sender, EventArgs e) {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            if (!allPlayersScoresDictionary.ContainsKey(clientId)) {
                allPlayersScoresDictionary[clientId] = 0;
            }
        }

        //! В чём разница вызова данного метода на NetworkManager_OnClientConnectedCallback и здесь. Столит ли их дублировать
        //UpdatePlayersScoresDictionaryServerRpc();
        //! Ошибка InvalidOperationException: An RPC called on a NetworkObject that is not in the spawned objects list. Please make sure the NetworkObject is spawned before calling RPCs.
        //! Нужно убрать прослушку ивента после удаления gameObject
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeliverItemScoreServerRpc(int itemCost = 1, ServerRpcParams serverRpcParams = default) {
        allPlayersScoresDictionary[serverRpcParams.Receive.SenderClientId] += itemCost;
        UpdatePlayersScoresDictionaryServerRpc(); //! Лучше локально увеличить значение у каждого клиента, чем пересоздавать словарь.

        OnItemDeliveredClientRpc();
    }

    [ClientRpc]
    public void OnItemDeliveredClientRpc() {
        OnItemDelivered?.Invoke(this, EventArgs.Empty); //! Это так неправильно что-ли для этого создавать отдельную функцию
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayersScoresDictionaryServerRpc() {
        CreatePlayersScoresDictionaryClientRpc();

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            //!Unnecessary check?
            //if (allPlayersScoresDictionary.ContainsKey(clientId)) {
                //connectedPlayersScoresDictionary[clientId] = allPlayersScoresDictionary[clientId];
                UpdatePlayersScoresDictionaryClientRpc(clientId, allPlayersScoresDictionary[clientId]);
            //}
        }
    }

    [ClientRpc]
    public void CreatePlayersScoresDictionaryClientRpc() {
        connectedPlayersScoresDictionary = new Dictionary<ulong, int>(); //! Вызвать это уже на клиенте
    }

    [ClientRpc]
    public void UpdatePlayersScoresDictionaryClientRpc(ulong clientId, int score) {
        connectedPlayersScoresDictionary[clientId] = score;
    }

    //! Не вызывается после смены сцены (
    //public override void OnNetworkDespawn() {
    //    Player.OnAnyPlayerSpawned -= Player_OnAnyPlayerSpawned;
    //    Debug.Log("Despawned");
    //}
}
