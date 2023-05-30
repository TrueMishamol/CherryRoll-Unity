using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayersScoresSingleUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI plaerScoreText;


    public void SetPlayerScore (KeyValuePair<ulong, int> clientScore) {
        playerNameText.text = PlayersStaticData.GetPlayerNameById(clientScore.Key);
        plaerScoreText.text = clientScore.Value.ToString();
    }
}
