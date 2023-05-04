using UnityEngine;
using UnityEngine.UI;

public class IngameMenuUI : MonoBehaviour {

    public static IngameMenuUI Instance { get; private set; }

    [SerializeField] private Button closeButton;

    private bool isIngameMenuOppened = false;

    private void Awake() {
        Instance = this;

        closeButton.onClick.AddListener(() => {
            SwitchOpenClose();
        });
    }

    private void Start() {
        GameInput.Instance.OnMenuOpenCloseAction += GameInput_OnMenuOpenCloseAction;

        Hide();
    }

    private void GameInput_OnMenuOpenCloseAction(object sender, System.EventArgs e) {
        SwitchOpenClose();
    }

    public void SwitchOpenClose() {
        isIngameMenuOppened = !isIngameMenuOppened;

        if (isIngameMenuOppened) {
            Show();
        } else {
            Hide();
        }
    }

    public void Show() {
        isIngameMenuOppened = true;
        gameObject.SetActive(true);
    }

    public void Hide() {
        isIngameMenuOppened = false;
        gameObject.SetActive(false);
    }
}
