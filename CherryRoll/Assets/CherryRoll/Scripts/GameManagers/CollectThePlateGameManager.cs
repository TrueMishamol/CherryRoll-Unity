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

    public Dictionary<ItemSO, int> requiredIngredientsDictionary;
    private Dictionary<ItemSO, int> collectedIngredientsDictionary;
    public Dictionary<ItemSO, int> localCollectedIngredientsDictionary;
    private int failedItemDeliveredAmount = 0;


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

        //if (!IsServer) return;

        //NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback; ;

        //UpdateIngredientsRecipeDictionaryServerRpc();
    }

    private void Start() {
        UpdateIngredientsRecipeDictionaryServerRpc();
        RecipeListUI.Instance.UpdateVisual(); //! Refactor?
    }

    //private void NetworkManager_OnClientConnectedCallback(ulong obj) {
    //    UpdateIngredientsRecipeDictionaryServerRpc();
    //}

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
                collectedIngredientsDictionary[itemSO]--;
                failedItemDeliveredAmount++;
            }
        } else {
            // Wrong item
            //! Add wrong item randomization
            failedItemDeliveredAmount++;
        }

        UpdateIngredientsRecipeDictionaryServerRpc();

        OnItemDeliveredClientRpc();
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

    //public override void OnDestroy() {
    //    NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallback;
    //}
}
