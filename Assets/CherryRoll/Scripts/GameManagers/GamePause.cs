using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GamePause : NetworkBehaviour {


    public static GamePause Instance { get; private set; }


    public event EventHandler OnLocalGamePaused;
    public event EventHandler OnLocalGameUnpaused;
    public event EventHandler OnMultiplayerGamePaused;
    public event EventHandler OnMultiplayerGameUnpaused;

    private bool isLocalGamePaused = false;
    private NetworkVariable<bool> isMultiplayerGamePaused = new NetworkVariable<bool>(false);
    private Dictionary<ulong, bool> playerPausedDictionary;
    private bool autoTestMultiplayerGamePauseState;


    private void Awake() {
        Instance = this;

        playerPausedDictionary = new Dictionary<ulong, bool>();
    }

    public override void OnNetworkSpawn() {
        isMultiplayerGamePaused.OnValueChanged += IsMultiplayerGamePaused_OnValueChanged;

        if (IsServer) {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }
    }

    private void IsMultiplayerGamePaused_OnValueChanged(bool previousValue, bool newValue) {
        if (isMultiplayerGamePaused.Value) {
            Time.timeScale = 0f;

            OnMultiplayerGamePaused?.Invoke(this, EventArgs.Empty);
        } else {
            Time.timeScale = 1f;

            OnMultiplayerGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId) {
        autoTestMultiplayerGamePauseState = true;
    }

    private void LateUpdate() {
        if (!IsServer) return;

        if (autoTestMultiplayerGamePauseState) {
            autoTestMultiplayerGamePauseState = false;
            TestMultiplayerGamePausedState();
        }
    }

    private void TestMultiplayerGamePausedState() {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            if (playerPausedDictionary.ContainsKey(clientId) && playerPausedDictionary[clientId]) {
                //^ This player is paused
                isMultiplayerGamePaused.Value = true;
                return;
            }
        }
        //^ All players are unpaused
        isMultiplayerGamePaused.Value = false;
    }

    public void SetLocalGamePaused(bool isLocalGamePaused) {
        this.isLocalGamePaused = isLocalGamePaused;
        UpdateLocalPauseGame();
    }

    private void UpdateLocalPauseGame() {
        if (isLocalGamePaused) {
            PauseGameServerRpc();

            OnLocalGamePaused?.Invoke(this, EventArgs.Empty);
        } else {
            UnpauseGameServerRpc();

            OnLocalGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default) {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;

        TestMultiplayerGamePausedState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UnpauseGameServerRpc(ServerRpcParams serverRpcParams = default) {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;

        TestMultiplayerGamePausedState();
    }

    public bool IsGamePaused() {
        return isMultiplayerGamePaused.Value;
    }
}
