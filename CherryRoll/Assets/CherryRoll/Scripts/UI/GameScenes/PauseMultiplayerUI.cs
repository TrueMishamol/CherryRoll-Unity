using UnityEngine;

public class PauseMultiplayerUI : MonoBehaviour {

    private void Start() {
        GamePause.Instance.OnMultiplayerGamePaused += PauseGameManager_OnMultiplayerGamePaused;
        GamePause.Instance.OnMultiplayerGameUnpaused += PauseGameManager_OnMultiplayerGameUnpaused;

        if (!GamePause.Instance.IsGamePaused()) {
            Hide();
        }
    }

    private void PauseGameManager_OnMultiplayerGameUnpaused(object sender, System.EventArgs e) {
        Hide();
    }

    private void PauseGameManager_OnMultiplayerGamePaused(object sender, System.EventArgs e) {
        Show();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
