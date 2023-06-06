using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsOtherUI : MonoBehaviour {


    public static OptionsOtherUI Instance { get; private set; }


    [SerializeField] private Button closeButton;


    private void Awake() {
        Instance = this;

        closeButton.onClick.AddListener(() => {
            Hide();
        });
    }

    private void Start() {
        Hide();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
