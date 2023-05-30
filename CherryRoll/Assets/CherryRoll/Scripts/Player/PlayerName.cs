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
        UpdatePlayerName();
    }

    public void ChangePlayerName(string newPlayerName) {
        PlayersStaticData.ChangePlayerName(newPlayerName, OwnerClientId);
        UpdatePlayerName(); //! Do I need this? Maybe extra
    }

    public void UpdatePlayerName() {
        playerDisplayName.text = PlayersStaticData.GetPlayerNameById(OwnerClientId).ToString();
    }
}
