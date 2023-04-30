using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterInteractableObject : BaseInteractableObject {

    [SerializeField] private ItemSO itemSO;
    //[SerializeField] private Transform flourPrefab;
    [SerializeField] private Transform itemHolder;

    public override void Interact(Player player) {
        Transform itemTransform = Instantiate(itemSO.prefab, itemHolder);
        itemTransform.localPosition = Vector3.zero;

        Debug.Log(itemTransform.GetComponent<Item>().GetItemSO().itemName);
    }

    //public bool HasItem() {
    //return item != null;
    //}
}
