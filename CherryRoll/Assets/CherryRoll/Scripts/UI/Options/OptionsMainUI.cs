using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMainUI : MonoBehaviour {
    

    public static OptionsMainUI Instance { get; private set; }


    [SerializeField] private Button closeButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button languageButton;
    [SerializeField] private Button soundsButton;
    [SerializeField] private Button otherButton;


    private void Awake() {
        Instance = this;

        closeButton.onClick.AddListener(() => {
            Hide();
        });

        controlsButton.onClick.AddListener(() => {
            OptionsControlsUI.Instance.Show();
        });

        languageButton.onClick.AddListener(() => {
            OptionsLanguageUI.Instance.Show();
        });

        soundsButton.onClick.AddListener(() => {
            OptionsSoundsUI.Instance.Show();
        });

        otherButton.onClick.AddListener(() => {
            OptionsOtherUI.Instance.Show();
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
