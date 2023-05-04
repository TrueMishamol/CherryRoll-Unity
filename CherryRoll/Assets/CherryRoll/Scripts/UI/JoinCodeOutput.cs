using System;
using TMPro;
using UnityEngine;

public class JoinCodeOutput : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI joinCodeOutputText;


    private void Awake() {
        // Originally events are listening on start, but also on the start we ran IngameMenuUI.Hide()
        NetworkHandleConnection.OnJoinCodeUpdated += NetworkHandleConnection_OnJoinCodeUpdated;
    }

    private void NetworkHandleConnection_OnJoinCodeUpdated(object sender, EventArgs e) {
        joinCodeOutputText.text = NetworkHandleConnection.JoinCode;
    }
}
