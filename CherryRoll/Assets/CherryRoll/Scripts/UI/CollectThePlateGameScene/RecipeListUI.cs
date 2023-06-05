using System.Collections.Generic;
using UnityEngine;

public class RecipeListUI : MonoBehaviour {


    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeItemTemplate;

    public static RecipeListUI Instance;


    private void Awake() {
        Instance = this;

        recipeItemTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        CollectThePlateGameManager.Instance.OnItemDelivered += CollectThePlateGameManager_OnItemDelivered;
        CollectThePlateGameManager.Instance.OnIngredientsRecipeDictionaryUpdated += CollectThePlateGameManager_OnIngredientsRecipeDictionaryUpdated;

        //UpdateVisual();
    }

    private void CollectThePlateGameManager_OnIngredientsRecipeDictionaryUpdated(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void CollectThePlateGameManager_OnItemDelivered(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    public void UpdateVisual() {
        foreach (Transform child in container) {
            if (child == recipeItemTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<ItemSO, int> itemCount in CollectThePlateGameManager.Instance.requiredIngredientsDictionary) {
            int collectedItemCount = CollectThePlateGameManager.Instance.localCollectedIngredientsDictionary[itemCount.Key];

            Transform recipeListSingleUITransform = Instantiate(recipeItemTemplate, container);
            recipeListSingleUITransform.gameObject.SetActive(true);
            recipeListSingleUITransform.GetComponent<RecipeListSingleUI>().SetItem(itemCount, collectedItemCount);
        }
    }
}
