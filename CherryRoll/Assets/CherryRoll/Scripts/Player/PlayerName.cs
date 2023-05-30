using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerName : NetworkBehaviour {


    [SerializeField] private TextMeshProUGUI playerDisplayName;


    public override void OnNetworkSpawn() {
        ChangePlayerName("");

        PlayersStaticData.OnPlayerNameChanged += PlayersStaticData_OnPlayerNameChanged;
    }

    private void PlayersStaticData_OnPlayerNameChanged(object sender, System.EventArgs e) {
        playerDisplayName.text = PlayersStaticData.GetPlayerNameById(OwnerClientId).ToString();
    }

    public void ChangePlayerName(string newPlayerName) {
        PlayersStaticData.ChangePlayerName(newPlayerName, OwnerClientId);
        playerDisplayName.text = PlayersStaticData.GetPlayerNameById(OwnerClientId).ToString();
    }

    public void UpdatePlayerName() {
        playerDisplayName.text = PlayersStaticData.GetPlayerNameById(OwnerClientId).ToString();
    }
}
