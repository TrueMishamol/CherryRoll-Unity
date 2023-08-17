using UnityEngine;

public class WaitingForOtherPlayersUI : MonoBehaviour {

    private void Start() {
        GameStateAndTimer.Instance.OnLocalPlayerReadyChanged += GameStateAndTimerManager_OnLocalPlayerReadyChanged;
        GameStateAndTimer.Instance.OnStateChanged += GameStateAndTimerManager_OnStateChanged;

        Hide();
    }

    private void GameStateAndTimerManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameStateAndTimer.Instance.IsCountdownToStartActive()) {
            Hide();
        }
    }

    private void GameStateAndTimerManager_OnLocalPlayerReadyChanged(object sender, System.EventArgs e) {
        if (GameStateAndTimer.Instance.IsLocalPlayerReady()) {
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
