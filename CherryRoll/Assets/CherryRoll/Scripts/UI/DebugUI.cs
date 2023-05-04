using UnityEngine;

public class DebugUI : MonoBehaviour {


    public static DebugUI Instance { get; private set; }

    [SerializeField] private Transform ingameDebugConsole;

    private bool isDebugMenuOpenned = false;


    private void Awake() {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        GameInput.Instance.OnDebugOpenCloseAction += GameInput_OnDebugOpenCloseAction;

        Hide();
    }

    private void GameInput_OnDebugOpenCloseAction(object sender, System.EventArgs e) {
        SwitchOpenClose();
    }

    public void SwitchOpenClose() {
        isDebugMenuOpenned = !isDebugMenuOpenned;

        if (isDebugMenuOpenned) {
            Show();
        } else {
            Hide();
        }
    }

    public void Show() {
        gameObject.SetActive(true);

        // set active not from the start but only after 1-st opening
        ingameDebugConsole.gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
