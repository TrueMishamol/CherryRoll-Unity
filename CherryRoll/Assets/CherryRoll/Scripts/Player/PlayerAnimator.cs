using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour {


    private const string IS_WALKING = "IsWalking";
    private const string ON_JUMP = "OnJump";

    [SerializeField] private PlayerMovement player;

    private Animator animator;


    private void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start() {
        player.OnJump += Player_OnJump;
    }

    private void Update() {
        if (!IsOwner) return;

        animator.SetBool(IS_WALKING, player.IsWalking());
    }

    private void Player_OnJump(object sender, System.EventArgs e) {
        animator.SetTrigger(ON_JUMP);
    }
}