using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerMenuUI : MonoBehaviour {

    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Awake() {
        hostButton.onClick.AddListener(() => {

        });
    }
}
