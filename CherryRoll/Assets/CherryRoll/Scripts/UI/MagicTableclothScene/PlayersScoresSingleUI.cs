using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayersScoresSingleUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI plaerScoreText;


    public void SetPlayerScore (KeyValuePair<ulong, int> clientScore) {
        playerNameText.text = clientScore.Key.ToString(); //! Get player name by id
        plaerScoreText.text = clientScore.Value.ToString();

        Debug.Log(NetworkManager.Singleton.ConnectedClients[clientScore.Key].PlayerObject.GetComponent<Player>().GetColor());
    }
}
