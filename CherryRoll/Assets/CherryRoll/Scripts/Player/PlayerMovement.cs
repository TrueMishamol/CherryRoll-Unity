using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour {

    // Game Input
    private GameInput gameInput;

    [SerializeField] private CharacterController characterController;

    // Walk
    private int walkSpeed = 3;
    private Vector3 walkDir;
    float playerRotationSpeed = 300f;
    private bool isWalking;

    // Gravity
    private Vector3 fallingVelocityVector;
    private float standingVelocityValue = 0;
    private float gravity = -20f;

    // Jump and Gravity
    private float jumpHeight = 0.1f;


    private void Start() {
        if (IsOwner) {
            gameInput = GameInput.Instance;
        }
    }

    private void FixedUpdate() {
        if (!IsOwner) return;

        HandleMovement();
        HandleJumpAndGravity();

        ApplyFinalMovements();
    }

    private void ApplyFinalMovements() {
        // Walk
        if (walkDir != Vector3.zero) {
            characterController.Move(walkDir * walkSpeed * Time.fixedDeltaTime);
        }

        // Jump
        characterController.Move(fallingVelocityVector);
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorSmoothed();
        walkDir = new Vector3(inputVector.x, 0, inputVector.y);

        isWalking = walkDir != Vector3.zero;

        // Rotates Player to face walkDir
        if (isWalking) {
            Quaternion toRotation = Quaternion.LookRotation(walkDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, playerRotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void HandleJumpAndGravity() {
        if (characterController.isGrounded && fallingVelocityVector.y < 0) {
            fallingVelocityVector.y = standingVelocityValue;
        }

        if (!characterController.isGrounded) {
            fallingVelocityVector.y += gravity * Time.fixedDeltaTime * Time.fixedDeltaTime; // sqare of time
        }

        if (gameInput.IsJumping() && characterController.isGrounded) {
            fallingVelocityVector.y = jumpHeight;
        }
    }

    public bool IsWalking() {
        return isWalking;
    }
}
