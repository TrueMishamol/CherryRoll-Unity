using UnityEngine;

public class LobbyButtonAnimator : MonoBehaviour {


    private const string PRESS = "Press";

    [SerializeField] private LobbyButton lobbyButton;

    [SerializeField] private Animator animator;


    private void Awake() {
        //!animator = GetComponentInChildren<Animator>();
    }

    private void Start() {
        lobbyButton.OnPlayerPressButton += LobbyButton_OnPlayerPressButton;
    }

    private void LobbyButton_OnPlayerPressButton(object sender, System.EventArgs e) {
        animator.SetTrigger(PRESS);
    }
}
