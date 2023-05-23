using UnityEngine;

public class TutorialUI : MonoBehaviour {

    private void Start() {
        GameStateAndTimerManager.Instance.OnLocalPlayerReadyChanged += GameStateAndTimerManager_OnLocalPlayerReadyChanged;

        UpdateVisual();

        if (!GameStateAndTimerManager.Instance.IsWaitingToStart()) {
            Hide();
        }
    }

    private void GameStateAndTimerManager_OnLocalPlayerReadyChanged(object sender, System.EventArgs e) {
        if (GameStateAndTimerManager.Instance.IsLocalPlayerReady()) {
            Hide();
        }
    }

    private void UpdateVisual() {
        //! Game name & detailed Description
        //! Also maybe button bindings
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
