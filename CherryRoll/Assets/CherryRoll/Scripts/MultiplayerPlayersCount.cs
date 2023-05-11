using System;
using Unity.Netcode;
using UnityEngine;

public class MultiplayerPlayersCount : NetworkBehaviour {


    public static MultiplayerPlayersCount Instance { get; private set; }

    public static int maxPlayerAmount = 10;


    private void Awake() {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public int GetPlayersCount() {
        try {
            return NetworkManager.Singleton.ConnectedClients.Count;
        } catch (NotServerException) {
            // If the host stops, then constantly occurs thiss exception Unity.Netcode.NotServerException: ConnectedClients should only be accessed on server
            Debug.Log("Server stopped");
            return 0;
        }
    }
}
