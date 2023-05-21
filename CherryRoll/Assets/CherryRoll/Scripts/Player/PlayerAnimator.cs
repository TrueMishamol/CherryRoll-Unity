using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour {


    private const string IS_WALKING = "IsWalking";
    private const string ON_JUMP = "OnJump";
    private const string ON_INTERACT = "OnInteract";

    [SerializeField] private Player player;
    private PlayerMovement playerMovement;


    private Animator animator;


    private void Awake() {
        playerMovement = player.GetComponent<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start() {
        playerMovement.OnJump += Player_OnJump;
        player.OnInteract += Player_OnInteract;
    }

    private void Update() {
        if (!IsOwner) return;

        animator.SetBool(IS_WALKING, playerMovement.IsWalking());
    }

    private void Player_OnJump(object sender, System.EventArgs e) {
        animator.SetTrigger(ON_JUMP);
    }

    private void Player_OnInteract(object sender, System.EventArgs e) {
        animator.SetTrigger(ON_INTERACT);
    }
}