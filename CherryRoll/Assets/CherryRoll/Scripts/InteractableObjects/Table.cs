using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Table : NetworkBehaviour, IInteractableObject {

    public void Interact(Player player) {
        if (player.HasItem()) {
            // Player is carrying something
            BaseGameManager.Instance.IncreasePlayerScoreByOneServerRpc();
            player.GetItem().DestroySelf();
            BaseGameManager.Instance.GetPlayersScores();
        }
    }
}
