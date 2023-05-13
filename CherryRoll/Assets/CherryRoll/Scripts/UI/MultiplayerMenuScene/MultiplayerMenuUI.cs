using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerMenuUI : MonoBehaviour {


    public static MultiplayerMenuUI Instance { get; private set; }


    [SerializeField] private Button hostButton;
    [SerializeField] private Button connectButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI joinCodeInputField;
    [SerializeField] private TextMeshProUGUI nameInputField;
    [SerializeField] private Button closeButton;


    private void Awake() {
        Instance = this;

        hostButton.onClick.AddListener(() => {
            MultiplayerConnection.Instance.CreateRelay();
        });

        connectButton.onClick.AddListener(() => {
            string joinCode;
            joinCode = joinCodeInputField.text.ToUpper();
            joinCode = joinCode.Remove(joinCode.Length - 1);  // Convert from TMPro. TMPro adds an invisible character at the end
            if (joinCode == "") return;
            MultiplayerConnection.Instance.UpdateJoinCode(joinCode);

            MultiplayerConnection.Instance.JoinRelay();
        });

        backButton.onClick.AddListener(() => {
            Hide();
            ChoosePlaymodeUI.Instance.Show();
        });

        closeButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start() {
        //Hide();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
