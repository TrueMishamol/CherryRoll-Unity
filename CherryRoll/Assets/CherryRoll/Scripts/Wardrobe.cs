using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wardrobe : MonoBehaviour
{
    public Color RandomizePlayerColor()
    {
        Color playerColor = new Color(Random.value, Random.value, Random.value);
        return playerColor;
    }
}