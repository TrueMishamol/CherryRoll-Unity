using System.Collections.Generic;
using UnityEngine;

public class RecipeListUI : MonoBehaviour {


    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeItemTemplate;


    private void Awake() {
        recipeItemTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        CollectThePlateGameManager.Instance.OnItemDelivered += CollectThePlateGameManager_OnItemDelivered;

        UpdateVisual();
    }

    private void CollectThePlateGameManager_OnItemDelivered(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach (Transform child in container) {
            if (child == recipeItemTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<ItemSO, int> itemCount in CollectThePlateGameManager.Instance.requiredIngredientsRecipeDictionary) {
            int collectedItemCount = CollectThePlateGameManager.Instance.currentIngredientsRecipeDictionary[itemCount.Key];

            Transform recipeListSingleUITransform = Instantiate(recipeItemTemplate, container);
            recipeListSingleUITransform.gameObject.SetActive(true);
            recipeListSingleUITransform.GetComponent<RecipeListSingleUI>().SetItem(itemCount, collectedItemCount);
        }
    }
}
