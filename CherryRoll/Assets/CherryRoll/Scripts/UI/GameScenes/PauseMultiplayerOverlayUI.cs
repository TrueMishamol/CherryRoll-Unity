using UnityEngine;

public class PauseMultiplayerOverlayUI : MonoBehaviour {

    private void Start() {
        GamePause.Instance.OnMultiplayerGamePaused += PauseGameManager_OnMultiplayerGamePaused;
        GamePause.Instance.OnMultiplayerGameUnpaused += PauseGameManager_OnMultiplayerGameUnpaused;
        Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;

        Hide();
    }

    private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e) {
        if (GamePause.Instance.IsGamePaused()) {
            Show();
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
