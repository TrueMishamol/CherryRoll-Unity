using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {


    private const string PLAYER_PREFS_BINDINGS = "InputBindings";


    public static GameInput Instance { get; private set; }


    private PlayerInputActions playerInputActions;


    //^ Smooth Moove Vector
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    [SerializeField] private float smoothInputSpeed = .4f;

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnMenuOpenCloseAction;


    //^ Bindings
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

        Menu,
        Menu_Gamepad,

        Close,
        Close_Gamepad,

        Select,
        Select_Gamepad,
    }

    public enum BindingTag {
        Keyboard,
        Gamepad
    }

    public class BindingClass {
        public string name { get; set; }
        public InputAction inputAction { get; set; }
        public int bindingIndex { get; set; }
        public BindingTag tag { get; set; }
    }

    private BindingClass selectedBinding;


    private void SetBindingVariable(Binding binding) {
        switch (binding) {
            default:
            case Binding.Move_Up:
                selectedBinding = new BindingClass {
                    name = "Move Up",
                    inputAction = playerInputActions.Player.Movement,
                    bindingIndex = 1,
                    tag = BindingTag.Keyboard,
                };
                break;
            case Binding.Move_Down:
                selectedBinding = new BindingClass {
                    name = "Move Down",
                    inputAction = playerInputActions.Player.Movement,
                    bindingIndex = 2,
                    tag = BindingTag.Keyboard,
                };
                break;
            case Binding.Move_Left:
                selectedBinding = new BindingClass {
                    name = "Move Left",
                    inputAction = playerInputActions.Player.Movement,
                    bindingIndex = 3,
                    tag = BindingTag.Keyboard,
                };
                break;
            case Binding.Move_Right:
                selectedBinding = new BindingClass {
                    name = "Move Right",
                    inputAction = playerInputActions.Player.Movement,
                    bindingIndex = 4,
                    tag = BindingTag.Keyboard,
                };
                break;
            case Binding.Move_Gamepad:
                selectedBinding = new BindingClass {
                    name = "Move Gamepad",
                    inputAction = playerInputActions.Player.Movement,
                    bindingIndex = 5,
                    tag = BindingTag.Gamepad,
                };
                break;
            case Binding.Jump:
                selectedBinding = new BindingClass {
                    name = "Jump",
                    inputAction = playerInputActions.Player.Jump,
                    bindingIndex = 0,
                    tag = BindingTag.Keyboard,
                };
                break;
            case Binding.Jump_Gamepad:
                selectedBinding = new BindingClass {
                    name = "Jump Gamepad",
                    inputAction = playerInputActions.Player.Jump,
                    bindingIndex = 1,
                    tag = BindingTag.Gamepad,
                };
                break;
            case Binding.Interact:
                selectedBinding = new BindingClass {
                    name = "Interact",
                    inputAction = playerInputActions.Player.Interact,
                    bindingIndex = 0,
                    tag = BindingTag.Keyboard,
                };
                break;
            case Binding.Interact_Gamepad:
                selectedBinding = new BindingClass {
                    name = "Interact Gamepad",
                    inputAction = playerInputActions.Player.Interact,
                    bindingIndex = 2,
                    tag = BindingTag.Gamepad,
                };
                break;
            case Binding.InteractAlternate:
                selectedBinding = new BindingClass {
                    name = "Interact Alternate",
                    inputAction = playerInputActions.Player.InteractAlternate,
                    bindingIndex = 0,
                    tag = BindingTag.Keyboard,
                };
                break;
            case Binding.InteractAlternate_Gamepad:
                selectedBinding = new BindingClass {
                    name = "Interact Alternate Gamepad",
                    inputAction = playerInputActions.Player.InteractAlternate,
                    bindingIndex = 2,
                    tag = BindingTag.Gamepad,
                };
                break;
            case Binding.Menu:
                selectedBinding = new BindingClass {
                    name = "Menu",
                    inputAction = playerInputActions.Player.Menu,
                    bindingIndex = 0,
                    tag = BindingTag.Keyboard,
                };
                break;
            case Binding.Menu_Gamepad:
                selectedBinding = new BindingClass {
                    name = "Menu Gamepad",
                    inputAction = playerInputActions.Player.Menu,
                    bindingIndex = 1,
                    tag = BindingTag.Gamepad,
                };
                break;
            case Binding.Close:
                selectedBinding = new BindingClass {
                    name = "Close",
                    inputAction = playerInputActions.Player.Close,
                    bindingIndex = 0,
                    tag = BindingTag.Keyboard,
                };
                break;
            case Binding.Close_Gamepad:
                selectedBinding = new BindingClass {
                    name = "Close Gamepad",
                    inputAction = playerInputActions.Player.Close,
                    bindingIndex = 1,
                    tag = BindingTag.Gamepad,
                };
                break;
            case Binding.Select:
                selectedBinding = new BindingClass {
                    name = "Select",
                    inputAction = playerInputActions.Player.Select,
                    bindingIndex = 0,
                    tag = BindingTag.Keyboard,
                };
                break;
            case Binding.Select_Gamepad:
                selectedBinding = new BindingClass {
                    name = "Select Gamepad",
                    inputAction = playerInputActions.Player.Select,
                    bindingIndex = 1,
                    tag = BindingTag.Gamepad,
                };
                break;
        };
    }


    private void Awake() {
        Instance = this;

        //^ Initialize all the variables for Input System
        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Menu.performed += MenuOpenClose_performed;

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
        SetBindingVariable(binding);

        return selectedBinding.inputAction.bindings[selectedBinding.bindingIndex].ToDisplayString();
    }

    public string GetBindingName(Binding binding) {
        SetBindingVariable(binding);

        return selectedBinding.name;
    }

    public void RebindBinding(Binding binding, Action onActionRebound) {
        playerInputActions.Player.Disable();

        SetBindingVariable(binding);

        InputAction inputAction = selectedBinding.inputAction;
        int bindingIndex = selectedBinding.bindingIndex;

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                callback.Dispose();
                playerInputActions.Player.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            })
            .Start();
    }

    public void ResetBindings() {
        playerInputActions.RemoveAllBindingOverrides();

        playerInputActions.Player.Enable();

        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
        PlayerPrefs.Save();
    }

    private void OnDestroy() {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Menu.performed -= MenuOpenClose_performed;

        playerInputActions.Dispose();
    }
}
