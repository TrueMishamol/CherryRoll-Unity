using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyButtonAnimation : MonoBehaviour {


    private const string PRESS = "Press";

    [SerializeField] private LobbyButton lobbyButton;

    [SerializeField] private Animator animator;


    private void Awake() {
        //animator = GetComponent<Animator>();
    }

    private void Start() {
        lobbyButton.OnPlayerPressButton += LobbyButton_OnPlayerPressButton;
    }

    private void LobbyButton_OnPlayerPressButton(object sender, System.EventArgs e) {
        animator.SetTrigger(PRESS);
    }
}
