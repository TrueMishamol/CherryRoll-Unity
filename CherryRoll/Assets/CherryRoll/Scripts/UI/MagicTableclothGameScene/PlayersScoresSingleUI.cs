using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayersScoresSingleUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI outputText;


    public void SetPlayerScore (KeyValuePair<ulong, int> clientScore) {
        string playerName = PlayersStaticData.Instance.GetPlayerNameById(clientScore.Key);

        outputText.text = playerName + "  " + clientScore.Value;
    }
}
