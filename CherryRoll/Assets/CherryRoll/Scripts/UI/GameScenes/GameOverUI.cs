using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {


    [SerializeField] private Button quitButton;


    private void Awake() {
        quitButton.onClick.AddListener(() => {
            //! To lobby
            //! Also display this button only on Host & on Client display Main Menu button
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start() {
        GameStateAndTimerManager.Instance.OnStateChanged += GameStateAndTimerManager_OnStateChanged;

        Hide();
    }

    private void GameStateAndTimerManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameStateAndTimerManager.Instance.IsGameOver()) {
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
