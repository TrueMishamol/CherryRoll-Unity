using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CollectThePlateGameManager : NetworkBehaviour {


    public static CollectThePlateGameManager Instance { get; private set; }

    public event EventHandler OnItemDelivered;
    public event EventHandler OnItemDeliveredSuccess;
    public event EventHandler OnItemDeliveredFailed;

    [SerializeField] private IngredientsRecipeSO ingredientsRecipeSO;

    public Dictionary<ItemSO, int> requiredIngredientsRecipeDictionary;
    public Dictionary<ItemSO, int> currentIngredientsRecipeDictionary;
    private int failedItemDeliveredAmount;


    private void Awake() {
        Instance = this;

    //    currentIngredientsRecipeDictionary = new Dictionary<ItemSO, int>();
    //}

    //private void Start() {
        requiredIngredientsRecipeDictionary = new Dictionary<ItemSO, int>();
        currentIngredientsRecipeDictionary = new Dictionary<ItemSO, int>();

        foreach (IngredientsRecipeSO.Recipe recipe in ingredientsRecipeSO.recipe) {
            requiredIngredientsRecipeDictionary[recipe.itemSO] = recipe.amount;
        }

        foreach (IngredientsRecipeSO.Recipe recipe in ingredientsRecipeSO.recipe) {
            currentIngredientsRecipeDictionary[recipe.itemSO] = 0;
        }

        Debug.Log(requiredIngredientsRecipeDictionary);
        Debug.Log(currentIngredientsRecipeDictionary);
    }

    public void DeliverItem(Item item) {

        ItemSO itemSO = item.GetItemSO();

        if (requiredIngredientsRecipeDictionary.ContainsKey(itemSO)) {
            if (requiredIngredientsRecipeDictionary[itemSO] > currentIngredientsRecipeDictionary[itemSO]) {
                // Correct item
                currentIngredientsRecipeDictionary[itemSO]++;
                Debug.Log("Correct item");
            } else {
                // Extra item
                currentIngredientsRecipeDictionary[itemSO]--;
                Debug.Log("Extra item");
            }
        } else {
            // Wrong item
            Debug.Log("Wrong item");
        }

        Debug.Log(requiredIngredientsRecipeDictionary);
        Debug.Log(currentIngredientsRecipeDictionary);

        //foreach (KeyValuePair<ItemSO, int> ingredient in currentIngredientsRecipeDictionary) {
        //    if (ingredient == currentIngredientsRecipeDictionary[]) {

        //    }
        //}

        OnItemDelivered?.Invoke(this, EventArgs.Empty);
    }
}
