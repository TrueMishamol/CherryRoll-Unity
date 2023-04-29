using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterInteractableObject : BaseInteractableObject {

    [SerializeField] private ItemSO itemSO;
    //[SerializeField] private Transform flourPrefab;
    [SerializeField] private Transform itemHolder;

    public override void Interact(Player player) {
        Transform flourTransform = Instantiate(itemSO.prefab, itemHolder);
        flourTransform.localPosition = Vector3.zero;
    }

    //public bool HasItem() {
    //return item != null;
    //}
}
