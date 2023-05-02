using UnityEngine;

public class Wardrobe : MonoBehaviour, IInteractableObject {

    public void Interact(Player player) {
        Color playerColor = new Color(Random.value, Random.value, Random.value);
        player.ChangePlayerColor(playerColor);
    }
}