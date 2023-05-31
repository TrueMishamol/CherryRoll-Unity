using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayersStaticData : NetworkBehaviour {


    public static PlayersStaticData Instance { get; private set; }


    private Dictionary<ulong, Player> playerDictionary = new Dictionary<ulong, Player>();
    private Dictionary<ulong, Color> playerColorDictionary = new Dictionary<ulong, Color>();
    private Dictionary<ulong, string> playerNameDictionary = new Dictionary<ulong, string>();

    public event EventHandler OnPlayerNameChanged;
    public event EventHandler OnPlayerColorChanged;


    private void Awake() {
        Instance = this;
    }

    //public override void OnNetworkSpawn() {
    //    Debug.Log("PlayersStaticData OnNetworkSpawn");
    //    UpdatePlayerColorDictionaryServerRpc();
    //}

    // Set Player Name

    public void SetPlayerNameById(string newPlayerName, ulong clientId) {
        SetPlayerNameByIdServerRpc(newPlayerName, clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNameByIdServerRpc(string newPlayerName, ulong clientId) {
        if (newPlayerName == "") {

            if (clientId == 0) {
                newPlayerName = "Baker";
            } else {
                newPlayerName = "Bun " + clientId;
            }
        }

        SetPlayerNameByIdClientRpc(newPlayerName, clientId);
    }

    [ClientRpc]
    private void SetPlayerNameByIdClientRpc(string name, ulong clientId) {
        playerNameDictionary[clientId] = name;
        OnPlayerNameChanged?.Invoke(null, EventArgs.Empty);
    }

    // Set Player Color

    public void SetPlayerColorById(Color newPlayerColor, ulong clientId) {
        SetPlayerColorByIdServerRpc(newPlayerColor, clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerColorByIdServerRpc(Color color, ulong clientId) {
        SetPlayerColorByIdClientRpc(color, clientId);
    }

    [ClientRpc]
    private void SetPlayerColorByIdClientRpc(Color color, ulong clientId) {
        playerColorDictionary[clientId] = color;
        Debug.Log(GetPlayerNameById(clientId) + " changes color to " + GetPlayerColorById(clientId));
        OnPlayerColorChanged?.Invoke(null, EventArgs.Empty);
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerColorDictionaryServerRpc(ulong clientId) {
        ClientRpcParams clientRpcParams = new ClientRpcParams {
            Send = new ClientRpcSendParams {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        //Dictionary<ulong, Color> playerColorDictionaryCopy = new Dictionary<ulong, Color>();
        //playerColorDictionaryCopy = playerColorDictionary;

        foreach (KeyValuePair<ulong, Color> playerColor in playerColorDictionary) {
            UpdatePlayerColorDictionaryClientRpc(playerColor.Value, playerColor.Key, clientRpcParams);
        }
        //! finish update - Update mesh
    }

    [ClientRpc]
    private void UpdatePlayerColorDictionaryClientRpc(Color color, ulong clientId, ClientRpcParams clientRpcParams = default) {
        if (IsServer) return;
        playerColorDictionary[clientId] = color;
        Debug.Log("UpdatePlayerColorDictionaryClientRpc " + OwnerClientId);
    }

    // SetPlayerById

    private void SetPlayerById(Player player, ulong clientId) {
        playerDictionary[clientId] = player;
    }

    // Get

    public string GetPlayerNameById(ulong clientId) {
        return playerNameDictionary[clientId];
    }

    public Color GetPlayerColorById(ulong clientId) {
        return playerColorDictionary[clientId];
    }

    public Player GetPlayerById(ulong clientId) {
        return playerDictionary[clientId];
    }
}
