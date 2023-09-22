using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerMenuUI : MonoBehaviour {


    public static MultiplayerMenuUI Instance { get; private set; }


    [SerializeField] private Button hostButton;
    [SerializeField] private Button connectButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI joinCodeInputField;
    //[SerializeField] private TextMeshProUGUI nameInputField;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button closeButton;


    private void Awake() {
        Instance = this;

        hostButton.onClick.AddListener(() => {
            MultiplayerConnection.Instance.CreateRelay();
        });

        connectButton.onClick.AddListener(() => {
            string joinCode;
            joinCode = joinCodeInputField.text.ToUpper();
            joinCode = joinCode.Remove(joinCode.Length - 1);  //^ Convert from TMPro. TMPro adds an invisible character at the end
            if (joinCode == "") return;
            MultiplayerConnection.Instance.UpdateJoinCode(joinCode);

            MultiplayerConnection.Instance.JoinRelay();
        });

        optionsButton.onClick.AddListener(() => {
            OptionsMainUI.Instance.Show();
        });

        closeButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MenuMainMenuScene);
        });

        nameInputField.onValueChanged.AddListener((string newValue) => {
            Debug.Log(nameInputField.text);
            LocalPlayerStaticData.Instance.SetLocalPlayerName(nameInputField.text);
        });
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
