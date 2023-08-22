using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameOverUI : MonoBehaviour {


    [SerializeField] protected TextMeshProUGUI wonLoseText;
    protected string wonText = "You won!";
    protected string loseText = "You lose...";

    [SerializeField] protected Button quitButton;
    [SerializeField] protected TextMeshProUGUI quitButtonText;
    protected string hostQuitText = "To Lobby";
    protected string clientQuitText = "Disconnect";


    protected virtual void Awake() {
        if (NetworkManager.Singleton.LocalClientId == NetworkManager.ServerClientId) {
            //^ Is Host
            quitButtonText.text = hostQuitText;

            quitButton.onClick.AddListener(() => {
                Loader.LoadNetwork(Loader.Scene.GameLobbyScene);
            });
        } else {
            //^ Is Client
            quitButton.gameObject.SetActive(false);

            //^ This button annoys players, so it is hidden for now
            //! Add a question alert-window "Are you shure you want to disconnect from the server?"
            //quitButtonText.text = clientQuitText;

            //quitButton.onClick.AddListener(() => {
            //    NetworkManager.Singleton.Shutdown();
            //    Loader.Load(Loader.Scene.MenuMainMenuScene);
            //});
        }
    }

    private void Start() {
        GameStateAndTimer.Instance.OnStateChanged += GameStateAndTimerManager_OnStateChanged;

        Hide();
    }

    private void GameStateAndTimerManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameStateAndTimer.Instance.IsGameOver()) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
        UpdateVisual();
        quitButton.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    protected virtual void UpdateVisual() {

    }
}


