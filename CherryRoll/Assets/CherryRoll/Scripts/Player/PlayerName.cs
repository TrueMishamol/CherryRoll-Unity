using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerName : NetworkBehaviour {


    [SerializeField] private TextMeshProUGUI playerDisplayName;


    public override void OnNetworkSpawn() {
        Debug.Log(OwnerClientId + " PlayerName spawned");

        PlayersStaticData.Instance.OnPlayerNameChanged += PlayersStaticData_OnPlayerNameChanged;

        PlayersStaticData.Instance.UpdatePlayerNameDictionaryServerRpc(OwnerClientId);
        UpdateLocalPlayerName();
    }

    private void PlayersStaticData_OnPlayerNameChanged(object sender, System.EventArgs e) {
        UpdateLocalPlayerName();
    }

    public void ChangePlayerName(string newPlayerName) {
        PlayersStaticData.Instance.SetPlayerNameById(newPlayerName, OwnerClientId);
    }

    private void UpdateLocalPlayerName() {
        string playerName;

        try {
            playerName = PlayersStaticData.Instance.GetPlayerNameById(OwnerClientId);
        } catch (KeyNotFoundException) {
            if (IsOwner) {
                PlayersStaticData.Instance.SetPlayerNameById(GenerateDefaultPlayerName(), OwnerClientId);
            }
            playerName = GenerateDefaultPlayerName(); //! In case no update
        }

        playerDisplayName.text = playerName;
    }

    public override void OnDestroy() {
        PlayersStaticData.Instance.OnPlayerNameChanged -= PlayersStaticData_OnPlayerNameChanged;
    }

    public string GenerateDefaultPlayerName() {
        string name;

        if (OwnerClientId == 0) {
            name = "Baker";
        } else {
            name = "Bun " + OwnerClientId;
        }

        return name;
    }
}
