using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MagicTableclothGameOverUI : MonoBehaviour {


    [SerializeField] private Button quitButton;

    [SerializeField] private Transform container;
    [SerializeField] private Transform playerTemplate;

    private int bestScore;


    private void Awake() {
        playerTemplate.gameObject.SetActive(false);

        quitButton.onClick.AddListener(() => {
            //! Also display this button only on Host & on Client display Main Menu button
            if (NetworkManager.Singleton.LocalClientId == NetworkManager.ServerClientId) {
                //^ Is Host
                Loader.LoadNetwork(Loader.Scene.GameLobbyScene);
            }
        });
    }

    private void Start() {
        GameStateAndTimerManager.Instance.OnStateChanged += GameStateAndTimerManager_OnStateChanged;

        Hide();
    }

    private void GameStateAndTimerManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameStateAndTimerManager.Instance.IsGameOver()) {
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

        foreach (KeyValuePair<ulong, int> clientScore in MagicTableclothGameManager.Instance.connectedPlayersScoresDictionary.OrderByDescending(key => key.Value)) {
            bool isBestScore = false;
            if (bestScore == -1) bestScore = clientScore.Value;
            if (clientScore.Value == bestScore) isBestScore = true;

            Transform gameOverSingleUI = Instantiate(playerTemplate, container);
            gameOverSingleUI.gameObject.SetActive(true);
            gameOverSingleUI.GetComponent<MagicTableclothGameOverSingleUI>().SetPlayerScore(clientScore, isBestScore);
        }
    }
}
