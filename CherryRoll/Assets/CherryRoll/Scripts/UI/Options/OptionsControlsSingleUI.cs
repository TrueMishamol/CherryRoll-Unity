using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsControlsSingleUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI bindingNameText;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;


    public void SetBindingToButton(GameInput.Binding binding) {
        button.onClick.AddListener(() => { RebindBinding(binding); });

        //string bind = GameInput.Instance.GetBindingText(binding);
        buttonText.text = GameInput.Instance.GetBindingText(binding);
        //buttonText.text = bind;

        try {
            bindingNameText.text = OptionsControlsUI.Instance.bindingsNamesDictionary[binding];
        } catch {
            bindingNameText.text = binding.ToString();
        }
    }

    private void RebindBinding(GameInput.Binding binding) {
        OptionsControlsUI.Instance.ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () => {
            OptionsControlsUI.Instance.HidePressToRebindKey();
            OptionsControlsUI.Instance.UpdateVisual();
        });
    }
}
