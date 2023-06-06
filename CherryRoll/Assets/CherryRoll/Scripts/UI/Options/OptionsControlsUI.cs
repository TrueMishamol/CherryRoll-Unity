using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsControlsUI : MonoBehaviour {


    public static OptionsControlsUI Instance { get; private set; }


    [SerializeField] private Button closeButton;
    [SerializeField] private Transform pressToRebindTransform;

    private List<GameInput.Binding> keyboardBindings = new List<GameInput.Binding>()
    {
        GameInput.Binding.Move_Up,
        GameInput.Binding.Move_Down,
        GameInput.Binding.Move_Left,
        GameInput.Binding.Move_Right,
        GameInput.Binding.Jump,
        GameInput.Binding.Interact,
        GameInput.Binding.InteractAlternate,
        GameInput.Binding.MenuOpenClose,
    };

    private List<GameInput.Binding> gamepadBindings = new List<GameInput.Binding>()
    {
        GameInput.Binding.Move_Gamepad,
        GameInput.Binding.Jump_Gamepad,
        GameInput.Binding.InteractAlternate_Gamepad,
        GameInput.Binding.MenuOpenClose_Gamepad,
    };

    [Header("Keyboard")]
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private Button pauseButton;

    [Header("Gamepad")]
    [SerializeField] private Button moveGamepadButton;
    [SerializeField] private Button jumpGamepadButton;
    [SerializeField] private Button interactGamepadButton;
    [SerializeField] private Button interactAlternateGamepadButton;
    [SerializeField] private Button pauseGamepadButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI moveUpButtonText;
    [SerializeField] private TextMeshProUGUI moveDownButtonText;
    [SerializeField] private TextMeshProUGUI moveLeftButtonText;
    [SerializeField] private TextMeshProUGUI moveRightButtonText;
    [SerializeField] private TextMeshProUGUI jumpButtonText;
    [SerializeField] private TextMeshProUGUI interactButtonText;
    [SerializeField] private TextMeshProUGUI interactAlternateButtonText;
    [SerializeField] private TextMeshProUGUI pauseButtonText;

    [SerializeField] private TextMeshProUGUI moveGamepadButtonText;
    [SerializeField] private TextMeshProUGUI jumpGamepadButtonText;
    [SerializeField] private TextMeshProUGUI interactGamepadButtonText;
    [SerializeField] private TextMeshProUGUI interactAlternateGamepadButtonText;
    [SerializeField] private TextMeshProUGUI pauseGamepadButtonText;


    private void Awake() {
        Instance = this;

        closeButton.onClick.AddListener(() => {
            Hide();
        });

        moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up); });
        moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down); });
        moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left); });
        moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right); });
        jumpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Jump); });
        interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        interactAlternateButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlternate); });
        pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MenuOpenClose); });

        moveGamepadButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Gamepad); });
        jumpGamepadButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Jump_Gamepad); });
        interactGamepadButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact_Gamepad); });
        interactAlternateGamepadButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlternate_Gamepad); });
        pauseGamepadButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MenuOpenClose_Gamepad); });
    }

    private void Start() {
        UpdateVisual();

        HidePressToRebindKey();
        Hide();
    }

    private void UpdateVisual() {
        moveUpButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        jumpButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Jump);
        interactButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MenuOpenClose);

        moveGamepadButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Gamepad);
        jumpGamepadButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Jump_Gamepad);
        interactGamepadButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Gamepad);
        interactAlternateGamepadButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate_Gamepad);
        pauseGamepadButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MenuOpenClose_Gamepad);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey() {
        pressToRebindTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey() {
        pressToRebindTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding) {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () => {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
