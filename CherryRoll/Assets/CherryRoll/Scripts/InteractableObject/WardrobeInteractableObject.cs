using UnityEngine;

public class WardrobeInteractableObject : BaseInteractableObject
{
    public override void Interact(Player player) {
        Color playerColor = new Color(Random.value, Random.value, Random.value);
        player.ChangePlayerColor(playerColor);
    }
}