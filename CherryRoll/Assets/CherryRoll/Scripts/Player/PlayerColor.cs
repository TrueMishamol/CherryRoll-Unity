using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerColor : NetworkBehaviour {


    [SerializeField] private List<SkinnedMeshRenderer> paintableMeshesList;


    public override void OnNetworkSpawn() {
        //ChangePlayerColor(new Color(1, 1, 1)); //! Зачем не понятно, учитывая что он будет сбрасываться. Аналогично с именем
        UpdatePlayerColor();

        PlayersStaticData.OnPlayerColorChanged += PlayersStaticData_OnPlayerColorChanged;
    }

    private void PlayersStaticData_OnPlayerColorChanged(object sender, System.EventArgs e) {
        UpdatePlayerColor();
    }

    public void ChangePlayerColor(Color newPlayerColor) {
        PlayersStaticData.ChangePlayerColor(newPlayerColor, OwnerClientId);

        //UpdatePlayerColor();
    }

    private void UpdatePlayerColor() {
        Color color;

        try {
            color = PlayersStaticData.GetPlayerColorById(OwnerClientId);
        } catch (KeyNotFoundException) {
            color = new Color(1, 1, 1);
        }

        foreach (SkinnedMeshRenderer paintableMeshes in paintableMeshesList) {
            paintableMeshes.material.color = color;
        }
    }

    public override void OnDestroy() {
        PlayersStaticData.OnPlayerColorChanged -= PlayersStaticData_OnPlayerColorChanged;
    }
}
