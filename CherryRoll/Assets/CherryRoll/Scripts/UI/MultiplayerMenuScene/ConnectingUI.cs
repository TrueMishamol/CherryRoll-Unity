using UnityEngine;
using UnityEngine.UI;

public class ConnectingUI : MonoBehaviour {


    [SerializeField] private Button mainMenuButton;

    private float countdownToShowMainMenuButton = 7f;


    private void Awake() {
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start() {
        MultiplayerConnection.Instance.OnTryingToJoinGame += MultiplayerConnection_OnTryingToJoinGame;
        MultiplayerConnection.Instance.OnFailedToJoinGame += MultiplayerConnection_OnFailedToJoinGame;
        MultiplayerConnection.Instance.OnStartingRelay += MultiplayerConnection_OnStartingRelay;

        mainMenuButton.gameObject.SetActive(false);
        Hide();
    }

    private void Update() {
        if (mainMenuButton.gameObject.activeSelf) return;

        countdownToShowMainMenuButton -= Time.deltaTime;
        if (countdownToShowMainMenuButton < 0f) {
            mainMenuButton.gameObject.SetActive(true);
        }
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
