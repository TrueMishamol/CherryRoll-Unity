using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public static class PlayersStaticData {


    private static Dictionary<ulong, Player> playerDictionary = new Dictionary<ulong, Player>();
    private static Dictionary<ulong, Color> playerColorDictionary = new Dictionary<ulong, Color>();
    private static Dictionary<ulong, string> playerNameDictionary = new Dictionary<ulong, string>();

    public static event EventHandler OnPlayerNameChanged;



    public static void ChangePlayerName(string newPlayerName, ulong clientId) {
        ChangePlayerNameServerRpc(newPlayerName, clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private static void ChangePlayerNameServerRpc(string newPlayerName, ulong clientId) {
        if (newPlayerName == "") {

            if (clientId == 0) {
                newPlayerName = "Baker";
            } else {
                newPlayerName = "Bun " + clientId;
            }
        }

        SetPlayerNameById(newPlayerName, clientId);
    }



    private static void SetPlayerNameById(string name, ulong clientId) {
        playerNameDictionary[clientId] = name;
        OnPlayerNameChanged?.Invoke(null, EventArgs.Empty);
    }

    private static void SetPlayerColorById(Color color, ulong clientId) {
        playerColorDictionary[clientId] = color;
    }

    private static void SetPlayerById(Player player, ulong clientId) {
        playerDictionary[clientId] = player;
    }

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
