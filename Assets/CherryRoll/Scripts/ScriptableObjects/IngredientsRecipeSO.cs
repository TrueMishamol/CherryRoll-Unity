using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class IngredientsRecipeSO : ScriptableObject {
    

    [SerializeField] public List<Recipe> recipe;


    [System.Serializable]
    public class Recipe {
        [SerializeField] public ItemSO itemSO;
        [SerializeField] public int amount;
    }
}
