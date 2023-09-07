using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeListSingleUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI outputText;


    public void SetItem(KeyValuePair<ItemSO, int> itemCount, int collectedItemCount) {
        ItemSO itemSO = itemCount.Key;

        int remainItemCount = itemCount.Value - collectedItemCount;
        outputText.text = itemSO.itemName + "  " + remainItemCount;

        if (collectedItemCount == itemCount.Value) {
            transform.gameObject.SetActive(false);
        }
    }
}
