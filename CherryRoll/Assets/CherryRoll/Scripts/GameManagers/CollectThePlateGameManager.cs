using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class CollectThePlateGameManager : NetworkBehaviour {


    public static CollectThePlateGameManager Instance { get; private set; }

    public event EventHandler OnItemDelivered;
    public event EventHandler OnItemDeliveredSuccess;
    public event EventHandler OnItemDeliveredFailed;
    public event EventHandler OnIngredientsRecipeDictionaryUpdated;

    [SerializeField] private IngredientsRecipeSO ingredientsRecipeSO;
    [SerializeField] private Transform knifePrefab;

    public Dictionary<ItemSO, int> requiredIngredientsDictionary;
    private Dictionary<ItemSO, int> collectedIngredientsDictionary;
    public Dictionary<ItemSO, int> localCollectedIngredientsDictionary;
    private int failedItemDeliveredAmount = 0;
    private int maxWrongItemPunishment = 3;


    private void Awake() {
        Instance = this;

        //! Optimize

        requiredIngredientsDictionary = new Dictionary<ItemSO, int>();
        collectedIngredientsDictionary = new Dictionary<ItemSO, int>();
        localCollectedIngredientsDictionary = new Dictionary<ItemSO, int>();

        foreach (IngredientsRecipeSO.Recipe recipe in ingredientsRecipeSO.recipe) {
            requiredIngredientsDictionary[recipe.itemSO] = recipe.amount;
            collectedIngredientsDictionary[recipe.itemSO] = 0;
            localCollectedIngredientsDictionary[recipe.itemSO] = 0;
        }
    }

    private void Start() {
        UpdateIngredientsRecipeDictionaryServerRpc();
        RecipeListUI.Instance.UpdateVisual(); //! Refactor?
    }

    public void DeliverItem(Item item) {
        DeliverItemServerRpc(item.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeliverItemServerRpc(NetworkObjectReference itemNetworkObjectReference) {
        itemNetworkObjectReference.TryGet(out NetworkObject itemNetworkObject);
        Item item = itemNetworkObject.GetComponent<Item>();
        ItemSO itemSO = item.GetItemSO();

        if (requiredIngredientsDictionary.ContainsKey(itemSO)) {
            if (requiredIngredientsDictionary[itemSO] > collectedIngredientsDictionary[itemSO]) {
                // Correct item
                collectedIngredientsDictionary[itemSO]++;
            } else {
                // Extra item
                WrongItemDelivered();
            }
        } else {
            // Wrong item
            WrongItemDelivered();
        }

        UpdateIngredientsRecipeDictionaryServerRpc();

        OnItemDeliveredClientRpc();
    }

    private void WrongItemDelivered() {
        int randomCount = UnityEngine.Random.Range(1, maxWrongItemPunishment);

        for (int i = 4; i > 0; i--) {
            ItemSO randomItemSO = collectedIngredientsDictionary.ElementAt(UnityEngine.Random.Range(0, collectedIngredientsDictionary.Count)).Key;

            collectedIngredientsDictionary[randomItemSO]--;
        }

        failedItemDeliveredAmount++;
        maxWrongItemPunishment++;

        SpawnKnifeServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnKnifeServerRpc() {
        Transform knifeTransform = Instantiate(knifePrefab);

        NetworkObject knifeNetworkObject = knifeTransform.GetComponent<NetworkObject>();
        knifeNetworkObject.Spawn(true);
    }

    [ClientRpc]
    private void OnItemDeliveredClientRpc() {
        OnItemDelivered?.Invoke(this, EventArgs.Empty);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateIngredientsRecipeDictionaryServerRpc() {
        // copy currentIngredientsRecipeDictionary from server to clients
        List<ItemSO> itemSOList = collectedIngredientsDictionary.Keys.ToList();
        foreach (KeyValuePair<ItemSO, int> itemSOCount in collectedIngredientsDictionary) {
            int itemIndex = itemSOList.IndexOf(itemSOCount.Key);
            UpdateIngredientsRecipeDictionaryClientRpc(itemIndex, itemSOCount.Value);
        }

        OnIngredientsRecipeDictionaryUpdatedClientRpc();
    }

    [ClientRpc]
    private void UpdateIngredientsRecipeDictionaryClientRpc(int itemIndex, int itemCount) {
        ItemSO itemSO = localCollectedIngredientsDictionary.ElementAt(itemIndex).Key;

        localCollectedIngredientsDictionary[itemSO] = itemCount;
    }

    [ClientRpc]
    private void OnIngredientsRecipeDictionaryUpdatedClientRpc() {
        OnIngredientsRecipeDictionaryUpdated?.Invoke(this, EventArgs.Empty);
    }
}
