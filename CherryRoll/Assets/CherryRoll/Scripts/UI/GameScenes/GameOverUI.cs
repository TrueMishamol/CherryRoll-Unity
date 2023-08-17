using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {


    [SerializeField] private Button quitButton;


    private void Awake() {
        quitButton.onClick.AddListener(() => {
            //! Also display this button only on Host & on Client display Main Menu button
            if (NetworkManager.Singleton.LocalClientId == NetworkManager.ServerClientId) {
                //^ Is Host
                Loader.LoadNetwork(Loader.Scene.GameLobbyScene);
            }
        });
    }

    private void Start() {
        GameStateAndTimer.Instance.OnStateChanged += GameStateAndTimerManager_OnStateChanged;

        Hide();
    }

    private void GameStateAndTimerManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameStateAndTimer.Instance.IsGameOver()) {
            Show();

            //! Show players' scores
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
        quitButton.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
