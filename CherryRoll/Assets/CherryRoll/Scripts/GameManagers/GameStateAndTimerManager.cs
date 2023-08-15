using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class GameStateAndTimerManager : NetworkBehaviour {


    public static GameStateAndTimerManager Instance { get; private set; }


    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalPlayerReadyChanged;


    private enum State {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private bool isLocalPlayerReady;
    private NetworkVariable<float> countdownToStartTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> gamePlayingTimerCurrent = new NetworkVariable<float>(0f);

    [SerializeField] private int minutesGamePlayingTimer = 3;
    [SerializeField] private int secondsGamePlayingTimer = 0;
    private float gamePlayingTimerMax;
    private Dictionary<ulong, bool> playerReadyDictionary;


    private void Awake() {
        Instance = this;

        gamePlayingTimerMax = minutesGamePlayingTimer * 60 + secondsGamePlayingTimer;

        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public override void OnNetworkSpawn() {
        state.OnValueChanged += State_OnValueChanged;
    }

    private void State_OnValueChanged(State previousValue, State newValue) {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetLocalPlayerReady() {
        if (state.Value == State.WaitingToStart) {
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);

            SetPlayerReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default) {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId]) {
                // This player is NOT ready
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady) {
            state.Value = State.CountdownToStart;
        }
    }

    private void Update() {
        if (!IsServer) return;

        switch (state.Value) {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                countdownToStartTimer.Value -= Time.deltaTime;
                if (countdownToStartTimer.Value < 0f) {
                    state.Value = State.GamePlaying;
                    gamePlayingTimerCurrent.Value = gamePlayingTimerMax;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimerCurrent.Value -= Time.deltaTime;
                if (gamePlayingTimerCurrent.Value < 0f) {
                    state.Value = State.GameOver;
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsWaitingToStart() {
        return state.Value == State.WaitingToStart;
    }

    public bool IsLocalPlayerReady() {
        return isLocalPlayerReady;
    }

    public bool IsCountdownToStartActive() {
        return state.Value == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer() {
        return countdownToStartTimer.Value;
    }

    public bool IsGamePlaying() {
        return state.Value == State.GamePlaying;
    }

    public float GetGamePlayingTimerNormalized() {
        return 1 - (gamePlayingTimerCurrent.Value / gamePlayingTimerMax);
    }

    public bool IsGameOver() {
        return state.Value == State.GameOver;
    }
}
