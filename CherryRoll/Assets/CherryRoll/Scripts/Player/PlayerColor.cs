using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerColor : NetworkBehaviour {


    [SerializeField] private List<SkinnedMeshRenderer> paintableMeshesList;

    [SerializeField] private Color defaultPlayerColor;


    public override void OnNetworkSpawn() {
        PlayersStaticData.Instance.OnPlayerColorChanged += PlayersStaticData_OnPlayerColorChanged;

        PlayersStaticData.Instance.UpdatePlayerColorDictionaryServerRpc(OwnerClientId);
        UpdateLocalPlayersColor();
    }

    private void PlayersStaticData_OnPlayerColorChanged(object sender, EventArgs e) {
        UpdateLocalPlayersColor();
    }

    public void ChangePlayerColor(Color newPlayerColor) {
        PlayersStaticData.Instance.SetPlayerColorById(newPlayerColor, OwnerClientId);
    }

    private void UpdateLocalPlayersColor() {
        Color color;

        try {
            color = PlayersStaticData.Instance.GetPlayerColorById(OwnerClientId);
        } catch (KeyNotFoundException) {
            if (IsOwner) {
                PlayersStaticData.Instance.SetPlayerColorById(defaultPlayerColor, OwnerClientId); //! Триггерит OnPlayerColorChanged => повторное UpdateLocalPlayersColor
            }
            color = defaultPlayerColor;
        }

        foreach (SkinnedMeshRenderer paintableMeshes in paintableMeshesList) {
            paintableMeshes.material.color = color;
        }
    }

    public override void OnDestroy() {
        PlayersStaticData.Instance.OnPlayerColorChanged -= PlayersStaticData_OnPlayerColorChanged;
    }
}
