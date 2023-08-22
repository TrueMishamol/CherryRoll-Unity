using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsControlsUI : MonoBehaviour {


    public static OptionsControlsUI Instance { get; private set; }


    [SerializeField] private Transform container;
    [SerializeField] private Transform controlButtonTemplate;
    [SerializeField] private Button closeButton;
    [SerializeField] private Transform pressToRebindTransform;
    [SerializeField] private Transform resetAllBindingsButtonTransform;
    [SerializeField] private Button resetAllBindingsButton;

    private void Awake() {
        Instance = this;

        controlButtonTemplate.gameObject.SetActive(false);

        closeButton.onClick.AddListener(() => {
            if (pressToRebindTransform.gameObject.activeSelf == false) {
                Hide();
            } else {
                HidePressToRebindKey();
            }
        });

        resetAllBindingsButton.onClick.AddListener(() => {
            ResetBindings();
        });
    }

    private void Start() {
        UpdateVisual();

        HidePressToRebindKey();
        Hide();
    }

    public void UpdateVisual() {
        foreach (Transform child in container) {
            if (child == controlButtonTemplate) continue;
            if (child == resetAllBindingsButtonTransform) continue;
            Destroy(child.gameObject);
        }

        foreach (GameInput.Binding binding in (GameInput.Binding[])Enum.GetValues(typeof(GameInput.Binding))) {
            Transform playerScoreSingleUITransform = Instantiate(controlButtonTemplate, container);
            playerScoreSingleUITransform.gameObject.SetActive(true);
            playerScoreSingleUITransform.GetComponent<OptionsControlsSingleUI>().SetBindingToButton(binding);
        }
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void ShowPressToRebindKey() {
        pressToRebindTransform.gameObject.SetActive(true);
    }

    public void HidePressToRebindKey() {
        pressToRebindTransform.gameObject.SetActive(false);
    }

    private void ResetBindings() {
        GameInput.Instance.ResetBindings();

        UpdateVisual();
    }
}
