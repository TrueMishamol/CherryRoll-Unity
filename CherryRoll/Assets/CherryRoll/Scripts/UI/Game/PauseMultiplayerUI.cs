using UnityEngine;

public class PauseMultiplayerUI : MonoBehaviour {

    private void Start() {
        PauseGameManager.Instance.OnMultiplayerGamePaused += PauseGameManager_OnMultiplayerGamePaused;
        PauseGameManager.Instance.OnMultiplayerGameUnpaused += PauseGameManager_OnMultiplayerGameUnpaused;

        Hide();
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
