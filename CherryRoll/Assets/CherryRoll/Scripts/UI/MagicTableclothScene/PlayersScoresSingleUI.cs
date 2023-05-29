using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayersScoresSingleUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI plaerScoreText;


    public void SetPlayerScore (KeyValuePair<ulong, int> clientScore) {
        //Debug.Log("Player " + clientScore.Key + " score is " + clientScore.Value);

        playerNameText.text = clientScore.Key.ToString(); //! Get player name by id
        plaerScoreText.text = clientScore.Value.ToString();
    }
}
