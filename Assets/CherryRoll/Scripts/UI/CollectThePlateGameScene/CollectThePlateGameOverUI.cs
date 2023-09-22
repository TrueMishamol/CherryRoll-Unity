using TMPro;
using UnityEngine;

public class CollectThePlateGameOverUI : BaseGameOverUI {
    

    [SerializeField] protected TextMeshProUGUI successText;
    [SerializeField] protected TextMeshProUGUI failedText;

    private string successItemDeliveredTitle = "Success: ";
    private string failedItemDeliveredTitle = "Failed: ";


    protected override void UpdateVisual() {
        base.UpdateVisual();

        if (GameCollectThePlateManager.Instance.CheckForWin()) {
            wonLoseText.text = wonText;
        } else {
            wonLoseText.text = loseText;
        }

        GameCollectThePlateManager.Instance.successItemDeliveredAmount.OnValueChanged += SuccessItemDeliveredAmount_OnValueChanged;

        successText.text = successItemDeliveredTitle + GameCollectThePlateManager.Instance.GetSuccessItemDeliveredAmount().ToString();
        failedText.text = failedItemDeliveredTitle + GameCollectThePlateManager.Instance.GetFailedItemDeliveredAmount().ToString();
    }

    private void SuccessItemDeliveredAmount_OnValueChanged(int previousValue, int newValue) {
        GameCollectThePlateManager.Instance.successItemDeliveredAmount.OnValueChanged -= SuccessItemDeliveredAmount_OnValueChanged;

        successText.text = successItemDeliveredTitle + GameCollectThePlateManager.Instance.GetSuccessItemDeliveredAmount().ToString();
        failedText.text = failedItemDeliveredTitle + GameCollectThePlateManager.Instance.GetFailedItemDeliveredAmount().ToString();
    }
}
