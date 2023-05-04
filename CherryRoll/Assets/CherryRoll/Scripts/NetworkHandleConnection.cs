using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class NetworkHandleConnection : NetworkBehaviour {


    public static NetworkHandleConnection Instance { get; private set; }

    public static event EventHandler OnJoinCodeUpdated;

    public static string JoinCode { get; private set; }


    private void Awake() {
        Instance = this;
    }

    private async void Start() {
        // Sends a request to Unity Services to initialize the API
        // With Async, the game does not freeze until response
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay() {
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
    }

    public async void JoinRelay() {
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

    public void UpdateJoinCode(string newJoinCode) {
        JoinCode = newJoinCode;
        OnJoinCodeUpdated?.Invoke(this, EventArgs.Empty);
    }
}
