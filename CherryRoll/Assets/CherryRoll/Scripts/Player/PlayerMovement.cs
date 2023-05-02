using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    // Game Input
    private GameObject gameInputHolder;
    private GameInput gameInput;
    private string gameInputHolderName = "GameInput";

    [SerializeField] private CharacterController characterController;

    // Walk
    [SerializeField] private int walkSpeed = 3;
    private Vector3 walkDir;
    [SerializeField] float playerRotationSpeed = 300f;

    // Gravity
    private Vector3 fallingVelocityVector;
    private float standingVelocityValue = 0;
    [SerializeField] private float gravity = -26f;

    // Jump and Gravity
    [SerializeField] private float jumpHeight = .03f;

    private void Start()
    {
        if (IsOwner)
        {
            // Game Input
            gameInputHolder = GameObject.Find(gameInputHolderName);
            gameInputHolder.TryGetComponent<GameInput>(out gameInput);
        }
    }

    private void Update()
    {
        if (!IsOwner) return;

        HandleMovement();
        HandleJumpAndGravity();

        ApplyFinalMovements();
    }

    private void ApplyFinalMovements()
    {
        // Walk
        if (walkDir != Vector3.zero)
        {
            characterController.Move(walkDir * walkSpeed * Time.deltaTime);
        }

        // Jump
        characterController.Move(fallingVelocityVector);
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorSmoothed();
        walkDir = new Vector3(inputVector.x, 0, inputVector.y);

        // Rotates Player to face walkDir
        if (walkDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(walkDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, playerRotationSpeed * Time.deltaTime);
        }
    }

    private void HandleJumpAndGravity()
    {
        if (characterController.isGrounded && fallingVelocityVector.y < 0)
        {
            fallingVelocityVector.y = standingVelocityValue;
        }

        if (!characterController.isGrounded)
        {
            fallingVelocityVector.y += gravity * Time.deltaTime * Time.deltaTime; // sqare of time
        }

        if (gameInput.IsJumping() && characterController.isGrounded)
        {
            fallingVelocityVector.y = jumpHeight;
        }
    }
}
