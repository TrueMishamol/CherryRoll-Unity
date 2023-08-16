using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagicTableclothGameOverSingleUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private Transform winnerOutline;

    private string playerName;
    private int playerScore;


    private void Awake() {
        winnerOutline.gameObject.SetActive(false);
    }

    public void SetPlayerScore(KeyValuePair<ulong, int> clientScore, bool isBestScore) {
        playerName = PlayersStaticData.Instance.GetPlayerNameById(clientScore.Key);
        playerScore = clientScore.Value;

        playerNameText.text = playerName;
        playerScoreText.text = playerScore.ToString();

        if (isBestScore) winnerOutline.gameObject.SetActive(true);
    }
}
