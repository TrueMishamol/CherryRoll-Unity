using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayersStaticData : NetworkBehaviour {


    public static PlayersStaticData Instance { get; private set; }


    private static Dictionary<ulong, Player> playerDictionary = new Dictionary<ulong, Player>();
    public static Dictionary<ulong, Color> playerColorDictionary = new Dictionary<ulong, Color>();
    private static Dictionary<ulong, string> playerNameDictionary = new Dictionary<ulong, string>();

    public static event EventHandler OnPlayerNameChanged;
    public static event EventHandler OnPlayerColorChanged;


    private void Awake() {
        Instance = this;
    }

    // Set Player Name

    public static void SetPlayerNameById(string newPlayerName, ulong clientId) {
        Instance.SetPlayerNameByIdServerRpc(newPlayerName, clientId);
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

    public static void SetPlayerColorById(Color newPlayerColor, ulong clientId) {
        Instance.SetPlayerColorByIdServerRpc(newPlayerColor, clientId);
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

    // SetPlayerById

    private static void SetPlayerById(Player player, ulong clientId) {
        playerDictionary[clientId] = player;
    }

    // Get

    public static string GetPlayerNameById(ulong clientId) {
        return playerNameDictionary[clientId];
    }

    public static Color GetPlayerColorById(ulong clientId) {
        return playerColorDictionary[clientId];
    }

    public static Player GetPlayerById(ulong clientId) {
        return playerDictionary[clientId];
    }
}
