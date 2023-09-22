using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayersScoresSingleUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI outputText;
    [SerializeField] private Transform winnerOverlay;


    private void Awake() {
        winnerOverlay.gameObject.SetActive(false);
    }

    public void SetPlayerScore (KeyValuePair<ulong, int> clientScore, bool isBestScore) {
        string playerName = PlayersStaticData.Instance.GetPlayerNameById(clientScore.Key);

        outputText.text = playerName + "  " + clientScore.Value;

        if (isBestScore) winnerOverlay.gameObject.SetActive(true);
    }
}
