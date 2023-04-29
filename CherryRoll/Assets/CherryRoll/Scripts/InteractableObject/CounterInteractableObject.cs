using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterInteractableObject : BaseInteractableObject {

    //private Item item;

    [SerializeField] private Transform flourPrefab;
    [SerializeField] private Transform itemHolder;

    public override void Interact(Player player) {
        Transform flourTransform = Instantiate(flourPrefab, itemHolder);
        flourTransform.localPosition = Vector3.zero;
    }

    //public bool HasItem() {
    //return item != null;
    //}
}
