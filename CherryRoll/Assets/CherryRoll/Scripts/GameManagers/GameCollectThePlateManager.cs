using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class GameCollectThePlateManager : NetworkBehaviour {


    public static GameCollectThePlateManager Instance { get; private set; }

    public event EventHandler OnItemDelivered;
    public event EventHandler OnItemDeliveredSuccess;
    public event EventHandler OnItemDeliveredFailed;
    public event EventHandler OnIngredientsRecipeDictionaryUpdated;

    [SerializeField] private IngredientsRecipeSO ingredientsRecipeSO;
    [SerializeField] private Transform knifePrefab;

    public Dictionary<ItemSO, int> requiredIngredientsDictionary;
    private Dictionary<ItemSO, int> collectedIngredientsDictionary;
    public Dictionary<ItemSO, int> localCollectedIngredientsDictionary;
    private int maxWrongItemPunishment = 3;
    private bool isWin = false;

    public NetworkVariable<int> successItemDeliveredAmount = new NetworkVariable<int>(0);
    private NetworkVariable<int> failedItemDeliveredAmount = new NetworkVariable<int>(0);


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

    public void DeliverItem(Item item, Player player) {
        DeliverItemServerRpc(item.GetNetworkObject(), player.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeliverItemServerRpc(NetworkObjectReference itemNetworkObjectReference, NetworkObjectReference playerNetworkObjectReference) {
        itemNetworkObjectReference.TryGet(out NetworkObject itemNetworkObject);
        Item item = itemNetworkObject.GetComponent<Item>();
        ItemSO itemSO = item.GetItemSO();

        if (requiredIngredientsDictionary.ContainsKey(itemSO)) {
            if (requiredIngredientsDictionary[itemSO] > collectedIngredientsDictionary[itemSO]) {
                //^ Correct item
                collectedIngredientsDictionary[itemSO]++;
                successItemDeliveredAmount.Value++;
                OnItemDeliveredSuccessClientRpc();
            } else {
                //^ Extra item
                WrongItemDelivered(playerNetworkObjectReference);
            }
        } else {
            //^ Wrong item
            WrongItemDelivered(playerNetworkObjectReference);
        }

        UpdateIngredientsRecipeDictionaryServerRpc();

        OnItemDeliveredClientRpc();

        if (CheckForWin()) {
            GameStateAndTimer.Instance.ChangeStateOnServer(GameStateAndTimer.State.GameOver);
        }
    }

    private void WrongItemDelivered(NetworkObjectReference playerNetworkObjectReference) {
        if (!IsServer) return;

        int randomCount = UnityEngine.Random.Range(1, maxWrongItemPunishment);

        for (int i = 4; i > 0; i--) {
            ItemSO randomItemSO = collectedIngredientsDictionary.ElementAt(UnityEngine.Random.Range(0, collectedIngredientsDictionary.Count)).Key;

            collectedIngredientsDictionary[randomItemSO]--;
        }

        failedItemDeliveredAmount.Value++;
        maxWrongItemPunishment++;

        OnItemDeliveredFailedClientRpc();
        SpawnKnifeServerRpc(playerNetworkObjectReference);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnKnifeServerRpc(NetworkObjectReference targetNetworkObjectReference) {
        Transform knifeTransform = Instantiate(knifePrefab);

        NetworkObject knifeNetworkObject = knifeTransform.GetComponent<NetworkObject>();
        knifeNetworkObject.Spawn(true);

        SetSpawnKnifePositionClientRpc(knifeNetworkObject, targetNetworkObjectReference);
    }

    //^ To avoid spawn position lag on clients, we should run spawn positioning not only on server
    [ClientRpc]
    private void SetSpawnKnifePositionClientRpc(NetworkObjectReference knifeNetworkObjectReference, NetworkObjectReference targetNetworkObjectReference) {
        knifeNetworkObjectReference.TryGet(out NetworkObject knifeNetworkObject);

        targetNetworkObjectReference.TryGet(out NetworkObject targetNetworkObject);
        Transform targetTransform = targetNetworkObject.gameObject.transform;

        knifeNetworkObject.transform.position = targetTransform.position + (Vector3.up * 10);
        knifeNetworkObject.transform.rotation = targetTransform.rotation;
    }


    //^ On Item Delivered

    [ClientRpc]
    private void OnItemDeliveredClientRpc() {
        OnItemDelivered?.Invoke(this, EventArgs.Empty);
    }

    [ClientRpc]
    private void OnItemDeliveredSuccessClientRpc() {
        OnItemDeliveredSuccess?.Invoke(this, EventArgs.Empty);
    }

    [ClientRpc]
    private void OnItemDeliveredFailedClientRpc() {
        OnItemDeliveredFailed?.Invoke(this, EventArgs.Empty);
    }



    [ServerRpc(RequireOwnership = false)]
    private void UpdateIngredientsRecipeDictionaryServerRpc() {
        //^ copy currentIngredientsRecipeDictionary from server to clients
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

    public bool CheckForWin() {
        //^ Only can run on server
        if (!IsServer) Debug.LogError("CheckForWin only can be run from the Server. Since it's values updates only on Server or updates later on Clients");

        if (isWin) return isWin;

        foreach (KeyValuePair<ItemSO, int> itemCount in requiredIngredientsDictionary) {
            int requiredItemCount = itemCount.Value;
            int collectedItemCount = localCollectedIngredientsDictionary[itemCount.Key];

            if (requiredItemCount - collectedItemCount != 0) return false;
        }
        isWin = true;

        return isWin;
    }

    public int GetSuccessItemDeliveredAmount() {
        return successItemDeliveredAmount.Value;
    }

    public int GetFailedItemDeliveredAmount() {
        return failedItemDeliveredAmount.Value;
    }
}
