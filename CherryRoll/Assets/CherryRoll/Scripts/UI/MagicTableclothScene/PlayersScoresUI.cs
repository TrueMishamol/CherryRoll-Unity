﻿using System.Collections.Generic;
using UnityEngine;

public class PlayersScoresUI : MonoBehaviour {


    [SerializeField] private Transform container;
    [SerializeField] private Transform playerScoreTemplate;


    private void Awake() {
        playerScoreTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        MagicTableclothGameManager.Instance.OnPlayersScoresDictionaryUpdated += MagicTableclothGameManager_OnPlayersScoresDictionaryUpdated;

        UpdateVisual();
    }

    private void MagicTableclothGameManager_OnPlayersScoresDictionaryUpdated(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach (Transform child in container) {
            if (child == playerScoreTemplate) continue;
            Destroy(child.gameObject);
        }
    
        foreach (KeyValuePair<ulong, int> clientScore in MagicTableclothGameManager.Instance.connectedPlayersScoresDictionary) {
            Transform playerScoreSingleUITransform = Instantiate(playerScoreTemplate, container);
            playerScoreSingleUITransform.gameObject.SetActive(true);
            playerScoreSingleUITransform.GetComponent<PlayersScoresSingleUI>().SetPlayerScore(clientScore);
        }
    }
}