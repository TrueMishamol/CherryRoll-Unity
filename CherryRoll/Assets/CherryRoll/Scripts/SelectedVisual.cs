using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedVisual : MonoBehaviour {

    [SerializeField] private BaseInteractableObject interactableObject;
    [SerializeField] private GameObject[] visualGameObjectArray;

    //private void Start() {
    //    Player.Instance.OnSelectedInteractableObjectChanged += Player_OnSelectedInteractableObjectChanged;
    //}

    //private void Player_OnSelectedInteractableObjectChanged(object sender, Player.OnSelectedInteractableObjectChangedEventArgs e) {

    //    Debug.Log("Selected object" + e);

    //    if (e.selectedInteractableObject == interactableObject) {
    //        Show();
    //    } else {
    //        Hide();
    //    }
    //}

    private void Show() {
        foreach (GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide() {
        foreach (GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(false);
        }
    }
}
