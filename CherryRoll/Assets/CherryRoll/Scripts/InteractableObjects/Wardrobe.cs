using Unity.Netcode;
using UnityEngine;

public class Wardrobe : NetworkBehaviour, IInteractableObject {

    public void Interact(Player player) {
        Color playerColor = new Color(Random.value, Random.value, Random.value);
        player.ChangePlayerColor(playerColor);
    }
}