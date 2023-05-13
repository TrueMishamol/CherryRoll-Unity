using System;
using TMPro;
using UnityEngine;

public class JoinCodeOutputTitle : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI joinCodeOutputText;


    private void Start() {
        MultiplayerConnection.OnJoinCodeUpdated += NetworkHandleConnection_OnJoinCodeUpdated;
        GameInput.Instance.OnMenuOpenCloseAction += GameInput_OnMenuOpenCloseAction;

        joinCodeOutputText.text = MultiplayerConnection.JoinCode;
    }

    private void GameInput_OnMenuOpenCloseAction(object sender, System.EventArgs e) {
         Hide();
    }

    private void NetworkHandleConnection_OnJoinCodeUpdated(object sender, EventArgs e) {
        joinCodeOutputText.text = MultiplayerConnection.JoinCode;
    }

    private void Hide() {
        gameObject.SetActive(false);

        MultiplayerConnection.OnJoinCodeUpdated -= NetworkHandleConnection_OnJoinCodeUpdated;
        GameInput.Instance.OnMenuOpenCloseAction -= GameInput_OnMenuOpenCloseAction;
    }

    private void OnDestroy() {
        MultiplayerConnection.OnJoinCodeUpdated -= NetworkHandleConnection_OnJoinCodeUpdated;
        GameInput.Instance.OnMenuOpenCloseAction -= GameInput_OnMenuOpenCloseAction;
    }
}
