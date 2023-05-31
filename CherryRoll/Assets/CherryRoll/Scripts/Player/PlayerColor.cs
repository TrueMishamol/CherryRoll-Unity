using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerColor : NetworkBehaviour {


    [SerializeField] private List<SkinnedMeshRenderer> paintableMeshesList;

    //private event EventHandler On


    public override void OnNetworkSpawn() {
        //if (IsOwner) {
        //    UpdateLocalPlayersColor();
        //}

        Debug.Log(OwnerClientId + " spawned");

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
        //    UpdatePlayerColorServerRpc();
        //}

        //[ServerRpc(RequireOwnership = false)]
        //private void UpdatePlayerColorServerRpc() {
        //    UpdatePlayerColorClientRpc();
        //}

        //[ClientRpc]
        //private void UpdatePlayerColorClientRpc() {

        Debug.Log("C UpdateLocalPlayerColor " + OwnerClientId);

        Color color;

        try {
            color = PlayersStaticData.Instance.GetPlayerColorById(OwnerClientId);
        } catch (KeyNotFoundException) {
            if (IsOwner) {
                PlayersStaticData.Instance.SetPlayerColorById(new Color(1, 1, 1), OwnerClientId); //! Триггерит OnPlayerColorChanged => повторное UpdateLocalPlayersColor
            }
            color = new Color(1, 1, 1);
        }

        foreach (SkinnedMeshRenderer paintableMeshes in paintableMeshesList) {
            paintableMeshes.material.color = color;
        }
    }

    //private static void UpdateLocalPlayersColor() {
    //    foreach (KeyValuePair<ulong, Color> playerColor in PlayersStaticData.playerColorDictionary) {
    //        Debug.Log("C UpdateLocalPlayerColor " + playerColor.Key);

    //        Color color = playerColor.Value;

    //        foreach (SkinnedMeshRenderer paintableMeshes in PlayersStaticData.GetPlayerById(playerColor.Key).GetComponent<PlayerColor>().paintableMeshesList) {
    //            paintableMeshes.material.color = color;
    //        }
    //    }
    //}

    public override void OnDestroy() {
        PlayersStaticData.Instance.OnPlayerColorChanged -= PlayersStaticData_OnPlayerColorChanged;
    }
}
