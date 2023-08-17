using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MagicTableclothGameOverUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI wonLoseText;
    private string wonText = "You won!";
    private string loseText = "You lose...";

    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI quitButtonText;
    private string hostQuitText = "To Lobby";
    private string clientQuitText = "Disconnect";

    [SerializeField] private Transform container;
    [SerializeField] private Transform playerTemplate;

    private int bestScore;


    private void Awake() {
        playerTemplate.gameObject.SetActive(false);

        if (NetworkManager.Singleton.LocalClientId == NetworkManager.ServerClientId) {
            //^ Is Host
            quitButtonText.text = hostQuitText;

            quitButton.onClick.AddListener(() => {
                Loader.LoadNetwork(Loader.Scene.GameLobbyScene);
            });
        } else {
            //^ Is Client
            quitButtonText.text = clientQuitText;

            quitButton.onClick.AddListener(() => {
                NetworkManager.Singleton.Shutdown();
                Loader.Load(Loader.Scene.MenuMainMenuScene);
            });
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

    private void UpdateVisual() {
        bestScore = -1;

        foreach (Transform child in container) {
            if (child == playerTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<ulong, int> clientScore in GameMagicTableclothManager.Instance.connectedPlayersScoresDictionary.OrderByDescending(key => key.Value)) {
            bool isBestScore = false;
            if (bestScore == -1) bestScore = clientScore.Value;
            if (clientScore.Value == bestScore) isBestScore = true;

            Transform gameOverSingleUI = Instantiate(playerTemplate, container);
            gameOverSingleUI.gameObject.SetActive(true);
            gameOverSingleUI.GetComponent<MagicTableclothGameOverSingleUI>().SetPlayerScore(clientScore, isBestScore);
        }

        if (bestScore == GameMagicTableclothManager.Instance.connectedPlayersScoresDictionary[NetworkManager.Singleton.LocalClientId]) {
            wonLoseText.text = wonText;
        } else {
            wonLoseText.text = loseText;
        }
    }
}
