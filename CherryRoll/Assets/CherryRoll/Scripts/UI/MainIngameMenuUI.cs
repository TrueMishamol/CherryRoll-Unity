using UnityEngine;
using UnityEngine.UI;

public class MainIngameMenuUI : MonoBehaviour {

    public static MainIngameMenuUI Instance { get; private set; }

    [SerializeField] private Button multiplayerButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitGameButton;


    private void Awake() {
        Instance = this;

        multiplayerButton.onClick.AddListener(() => {
            Hide();
            NetworkUI.Instance.Show();
        });
    }

    private void Start() {
        Hide();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
