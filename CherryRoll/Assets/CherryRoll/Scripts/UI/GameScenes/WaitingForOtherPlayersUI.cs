using UnityEngine;

public class WaitingForOtherPlayersUI : MonoBehaviour {

    private void Start() {
        GameStateAndTimerManager.Instance.OnLocalPlayerReadyChanged += GameStateAndTimerManager_OnLocalPlayerReadyChanged;
        GameStateAndTimerManager.Instance.OnStateChanged += GameStateAndTimerManager_OnStateChanged;

        Hide();
    }

    private void GameStateAndTimerManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameStateAndTimerManager.Instance.IsCountdownToStartActive()) {
            Hide();
        }
    }

    private void GameStateAndTimerManager_OnLocalPlayerReadyChanged(object sender, System.EventArgs e) {
        if (GameStateAndTimerManager.Instance.IsLocalPlayerReady()) {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
