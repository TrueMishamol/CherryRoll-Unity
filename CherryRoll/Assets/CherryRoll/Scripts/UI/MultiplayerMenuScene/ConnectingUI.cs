using UnityEngine;

public class ConnectingUI : MonoBehaviour {

    private void Start() {
        MultiplayerConnection.Instance.OnTryingToJoinGame += MultiplayerConnection_OnTryingToJoinGame;
        MultiplayerConnection.Instance.OnFailedToJoinGame += MultiplayerConnection_OnFailedToJoinGame;
        MultiplayerConnection.Instance.OnStartingRelay += MultiplayerConnection_OnStartingRelay;

        Hide();
    }

    private void MultiplayerConnection_OnStartingRelay(object sender, System.EventArgs e) {
        Show();
    }

    private void MultiplayerConnection_OnFailedToJoinGame(object sender, System.EventArgs e) {
        Hide();
    }

    private void MultiplayerConnection_OnTryingToJoinGame(object sender, System.EventArgs e) {
        Show();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        MultiplayerConnection.Instance.OnTryingToJoinGame -= MultiplayerConnection_OnTryingToJoinGame;
        MultiplayerConnection.Instance.OnFailedToJoinGame -= MultiplayerConnection_OnFailedToJoinGame;
    }
}
