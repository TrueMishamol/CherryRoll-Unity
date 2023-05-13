using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {


    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActions;

    // Smooth Moove Vector
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    [SerializeField] private float smoothInputSpeed = .4f;

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnMenuOpenCloseAction;
    public event EventHandler OnDebugOpenCloseAction;


    private void Awake() {
        if (Instance != null & Instance != this) {
            Destroy(Instance.gameObject);
        }
        Instance = this;

        // Initialize all the variables for Input System
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.MenuOpenClose.performed += MenuOpenClose_performed;
        playerInputActions.Player.DebugOpen.performed += DebugOpen_performed;

        DontDestroyOnLoad(gameObject);
    }

    private void DebugOpen_performed(InputAction.CallbackContext obj) {
        OnDebugOpenCloseAction?.Invoke(this, EventArgs.Empty);
    }

    private void MenuOpenClose_performed(InputAction.CallbackContext obj) {
        OnMenuOpenCloseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(InputAction.CallbackContext obj) {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public bool IsJumping() {
        bool isJumping = playerInputActions.Player.Jump.ReadValue<float>() > 0.5f;
        return isJumping;
    }

    public Vector2 GetMovementVectorSmoothed() {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        currentInputVector = Vector2.SmoothDamp(currentInputVector, inputVector, ref smoothInputVelocity, smoothInputSpeed);

        return inputVector;
    }

    private void OnDestroy() {
        if (Instance == this) {
            playerInputActions.Player.Interact.performed -= Interact_performed;
            playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
            playerInputActions.Player.MenuOpenClose.performed -= MenuOpenClose_performed;
            playerInputActions.Player.DebugOpen.performed -= DebugOpen_performed;
        }
    }
}
