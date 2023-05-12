using Unity.Netcode;
using UnityEngine;

public class MultiplayerPlayersCount : NetworkBehaviour {


    public static MultiplayerPlayersCount Instance { get; private set; }

    private NetworkVariable<int> playersCount = new NetworkVariable<int>(0);

    public static int maxPlayerAmount = 10;


    private void Awake() {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public int GetPlayersCount() {
        UpdatePlayersCount();
        return playersCount.Value;
    }

    public void UpdatePlayersCount() {
        if (!IsServer) return;

        try {
            playersCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
        } catch (NotServerException) {
            // If the host stops, then constantly occurs thiss exception Unity.Netcode.NotServerException: ConnectedClients should only be accessed on server
            Debug.Log("Server stopped");
            playersCount.Value = 0;
        }
    }


}
