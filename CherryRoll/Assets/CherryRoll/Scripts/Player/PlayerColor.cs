using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerColor : NetworkBehaviour {


    [SerializeField] private List<SkinnedMeshRenderer> paintableMeshesList;


    public override void OnNetworkSpawn() {
        if (IsOwner) {
            UpdateLocalPlayerColor();
        }

        PlayersStaticData.OnPlayerColorChanged += PlayersStaticData_OnPlayerColorChanged;
    }

    private void PlayersStaticData_OnPlayerColorChanged(object sender, System.EventArgs e) {
        UpdateLocalPlayerColor();
    }

    public void ChangePlayerColor(Color newPlayerColor) {
        PlayersStaticData.SetPlayerColorById(newPlayerColor, OwnerClientId);
    }

    public static void UpdateLocalPlayerColor() {
        UpdatePlayerColorServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePlayerColorServerRpc() {
        UpdatePlayerColorClientRpc();
    }

    [ClientRpc]
    private void UpdatePlayerColorClientRpc() {
        Debug.Log("C UpdatePlayerColorClientRpc " + OwnerClientId);

        Color color;

        try {
            color = PlayersStaticData.GetPlayerColorById(OwnerClientId);
        } catch (KeyNotFoundException) {
            PlayersStaticData.SetPlayerColorById(new Color(1, 1, 1), OwnerClientId);
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
