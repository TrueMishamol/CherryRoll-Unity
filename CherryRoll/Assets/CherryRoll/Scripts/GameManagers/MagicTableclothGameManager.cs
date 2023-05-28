using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MagicTableclothGameManager : NetworkBehaviour {


    public static MagicTableclothGameManager Instance { get; private set; }

    public event EventHandler OnItemDelivered;

    public NetworkVariable<Dictionary<ulong, int>> playersScoresDictionary = new NetworkVariable<Dictionary<ulong, int>>(new Dictionary<ulong, int>());


    private void Awake() {
        Instance = this;

        playersScoresDictionary = new NetworkVariable<Dictionary<ulong, int>>();

        //!if (!IsServer) return; //! Не работает должным образом и возвращает даже если сервер

        playersScoresDictionary.Value = new Dictionary<ulong, int>();
        Debug.Log("playersScoresDictionary.Value = new Dictionary<ulong, int>();");
    }

    private void Start() {
        if (!IsServer) return;

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            playersScoresDictionary.Value[clientId] = 0;
        }

        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;

        Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;

        // Присваиванеи значения 0 для всех клиентов, уже подключенных на сервере
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            playersScoresDictionary.Value[clientId] = 0;
        }
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId) {
        // Инициализация словаря на клиенте
        playersScoresDictionary.Value[clientId] = 0;
    }

    private void Player_OnAnyPlayerSpawned(object sender, EventArgs e) {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            if (!playersScoresDictionary.Value.ContainsKey(clientId)) {
                playersScoresDictionary.Value[clientId] = 0;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeliverItemScoreServerRpc(int itemCost = 1, ServerRpcParams serverRpcParams = default) {
        playersScoresDictionary.Value[serverRpcParams.Receive.SenderClientId] += itemCost;

        OnItemDelivered?.Invoke(this, EventArgs.Empty);
    }
}
