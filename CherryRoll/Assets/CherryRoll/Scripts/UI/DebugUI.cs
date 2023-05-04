using UnityEngine;

public class DebugUI : MonoBehaviour {

    public static DebugUI Instance { get; private set; }

    private bool isDebugMenuOpenned = false;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GameInput.Instance.OnDebugOpenCloseAction += GameInput_OnDebugOpenCloseAction; ;

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
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
