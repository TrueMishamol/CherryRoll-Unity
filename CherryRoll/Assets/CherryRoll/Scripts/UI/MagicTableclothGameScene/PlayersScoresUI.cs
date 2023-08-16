using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersScoresUI : MonoBehaviour {


    [SerializeField] private Transform container;
    [SerializeField] private Transform playerScoreTemplate;

    private int bestScore;


    private void Awake() {
        playerScoreTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        MagicTableclothGameManager.Instance.OnPlayersScoresDictionaryUpdated += MagicTableclothGameManager_OnPlayersScoresDictionaryUpdated;
        PlayersStaticData.Instance.OnPlayerNameChanged += PlayersStaticData_OnPlayerNameChanged;

        UpdateVisual();
    }

    private void PlayersStaticData_OnPlayerNameChanged(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void MagicTableclothGameManager_OnPlayersScoresDictionaryUpdated(object sender, System.EventArgs e) {
        try {
            UpdateVisual();
        } catch (KeyNotFoundException) {
            //^ It means that player just joined & playername dictionary is not updated yet. 
        }
    }

    private void UpdateVisual() {
        bestScore = -1;

        foreach (Transform child in container) {
            if (child == playerScoreTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<ulong, int> clientScore in MagicTableclothGameManager.Instance.connectedPlayersScoresDictionary.OrderByDescending(key => key.Value)) {
            bool isBestScore = false;
            if (bestScore == -1) bestScore = clientScore.Value;
            if (clientScore.Value == bestScore & bestScore != 0) isBestScore = true;

            Transform playerScoreSingleUITransform = Instantiate(playerScoreTemplate, container);
            playerScoreSingleUITransform.gameObject.SetActive(true);
            playerScoreSingleUITransform.GetComponent<PlayersScoresSingleUI>().SetPlayerScore(clientScore, isBestScore);
        }
    }
}
