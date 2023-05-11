using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerConnection : NetworkBehaviour {


    public static MultiplayerConnection Instance { get; private set; }

    public static event EventHandler OnJoinCodeUpdated;
    public static event EventHandler OnRelayStarted;
    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedToJoinGame;

    public static string JoinCode { get; private set; }


    private void Awake() {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private async void Start() {

        // Sends a request to Unity Services to initialize the API
        // With Async, the game does not freeze until response
        if (UnityServices.State == 0) {
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += () => {
                Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
            };
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async void CreateRelay() {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;

        try {
            // Creating Allocation on Relay
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(49); // 50 players

            // Getting the joinCode to join Allocation. joinCode is for Friends
            string joinCode;
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            UpdateJoinCode(joinCode);

            Debug.Log("Join Code " + JoinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
        } catch (RelayServiceException e) {
            Debug.Log(e);
        }

        OnRelayStarted?.Invoke(this, EventArgs.Empty);

        Loader.LoadNetwork(Loader.Scene.LobbyScene);
    }

    public async void JoinRelay() {
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;

        try {
            Debug.Log("Joining relay with " + JoinCode);

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(JoinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse) {
        bool canJoinOnGameStarted = true; //! Move to options

        if (canJoinOnGameStarted == false & 
            SceneManager.GetActiveScene().name != Loader.Scene.LobbyScene.ToString()) 
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started";
            return;
        }

        if (NetworkManager.Singleton.ConnectedClientsIds.Count > MultiplayerPlayersCount.maxPlayerAmount) {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full";
            return;
        }

        connectionApprovalResponse.Approved = true;
    }

    public void UpdateJoinCode(string newJoinCode) {
        JoinCode = newJoinCode;
        OnJoinCodeUpdated?.Invoke(this, EventArgs.Empty);
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId) {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
    }
}
