using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {


    private const string PLAYER_PREFS_BINDINGS = "InputBindings";


    public static GameInput Instance { get; private set; }


    private PlayerInputActions playerInputActions;

    public enum Binding {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Move_Gamepad,

        Jump,
        Jump_Gamepad,

        Interact,
        Interact_Gamepad,
        InteractAlternate,
        InteractAlternate_Gamepad,

        MenuOpenClose,
        MenuOpenClose_Gamepad,
    }

    // Smooth Moove Vector
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    [SerializeField] private float smoothInputSpeed = .4f;

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnMenuOpenCloseAction;
    public event EventHandler OnDebugOpenCloseAction;
    public event EventHandler OnBindingRebind; //! Пока нигде не используется


    private void Awake() {
        if (Instance != null & Instance != this) {
            Destroy(Instance.gameObject); //! Не уверен что это ещё нужно
        }
        Instance = this;

        // Initialize all the variables for Input System
        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.MenuOpenClose.performed += MenuOpenClose_performed;

        DontDestroyOnLoad(gameObject);
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

    public string GetBindingText(Binding binding) {
        switch (binding) {
            default:
            case Binding.Move_Up:
                return playerInputActions.Player.Movement.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Movement.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Movement.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Movement.bindings[4].ToDisplayString();
            case Binding.Move_Gamepad:
                return playerInputActions.Player.Movement.bindings[5].ToDisplayString();

            case Binding.Jump:
                return playerInputActions.Player.Movement.bindings[0].ToDisplayString();
            case Binding.Jump_Gamepad:
                return playerInputActions.Player.Movement.bindings[1].ToDisplayString();

            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.Interact_Gamepad:
                return playerInputActions.Player.Interact.bindings[2].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.InteractAlternate_Gamepad:
                return playerInputActions.Player.InteractAlternate.bindings[2].ToDisplayString();

            case Binding.MenuOpenClose:
                return playerInputActions.Player.MenuOpenClose.bindings[0].ToDisplayString();
            case Binding.MenuOpenClose_Gamepad:
                return playerInputActions.Player.MenuOpenClose.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound) {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding) {
            default:
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Movement;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Movement;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Movement;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Movement;
                bindingIndex = 4;
                break;
            case Binding.Move_Gamepad:
                inputAction = playerInputActions.Player.Movement;
                bindingIndex = 5;
                break;
            case Binding.Jump:
                inputAction = playerInputActions.Player.Jump;
                bindingIndex = 0;
                break;
            case Binding.Jump_Gamepad:
                inputAction = playerInputActions.Player.Jump;
                bindingIndex = 1;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.Interact_Gamepad:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 2;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate_Gamepad:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 2;
                break;
            case Binding.MenuOpenClose:
                inputAction = playerInputActions.Player.MenuOpenClose;
                bindingIndex = 0;
                break;
            case Binding.MenuOpenClose_Gamepad:
                inputAction = playerInputActions.Player.MenuOpenClose;
                bindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                callback.Dispose();
                playerInputActions.Player.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }

    private void OnDestroy() {
        if (Instance == this) { //! И не уверен нужна ли эта проверка
            playerInputActions.Player.Interact.performed -= Interact_performed;
            playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
            playerInputActions.Player.MenuOpenClose.performed -= MenuOpenClose_performed;
        }

        playerInputActions.Dispose();
    }
}
