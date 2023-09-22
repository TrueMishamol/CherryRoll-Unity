using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class MagicTableclothGameOverUI : BaseGameOverUI {


    [SerializeField] private Transform container;
    [SerializeField] private Transform playerTemplate;

    private int bestScore;


    protected override void Awake() {
        base.Awake();

        playerTemplate.gameObject.SetActive(false);
    }

    protected override void UpdateVisual() {
        base.UpdateVisual();
        
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
