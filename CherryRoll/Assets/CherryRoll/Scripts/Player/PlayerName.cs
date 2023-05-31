using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerName : NetworkBehaviour {


    [SerializeField] private TextMeshProUGUI playerDisplayName;


    public override void OnNetworkSpawn() {
        UpdatePlayerName();

        PlayersStaticData.OnPlayerNameChanged += PlayersStaticData_OnPlayerNameChanged;
    }

    private void PlayersStaticData_OnPlayerNameChanged(object sender, System.EventArgs e) {
        UpdatePlayerName();
    }

    public void ChangePlayerName(string newPlayerName) {
        PlayersStaticData.SetPlayerNameById(newPlayerName, OwnerClientId);
    }

    private void UpdatePlayerName() {
        UpdatePlayerNameServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePlayerNameServerRpc() {
        UpdatePlayerNameClientRpc();
    }

    [ClientRpc]
    private void UpdatePlayerNameClientRpc() {
        Debug.Log("N UpdatePlayerNameClientRpc");

        string playerName;

        try {
            playerName = PlayersStaticData.GetPlayerNameById(OwnerClientId);
        } catch (KeyNotFoundException) {
            PlayersStaticData.SetPlayerNameById("", OwnerClientId);
            playerName = PlayersStaticData.GetPlayerNameById(OwnerClientId);
        }

        playerDisplayName.text = PlayersStaticData.GetPlayerNameById(OwnerClientId);
    }

}
