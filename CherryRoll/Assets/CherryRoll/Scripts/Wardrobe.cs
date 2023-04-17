using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wardrobe : BaseInteractableObject
{
    public override void Interact(Player player) {
        Color playerColor = new Color(Random.value, Random.value, Random.value);
        player.ChangePlayerColor(playerColor);
    }
}