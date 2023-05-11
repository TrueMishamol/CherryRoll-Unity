using UnityEngine;
using UnityEngine.UI;

public class ChoosePlaymodeUI : MonoBehaviour {


    public static ChoosePlaymodeUI Instance { get; private set; }

    [SerializeField] private Button helpButton;
    //[SerializeField] private Button lobbyButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button connectButton;
    //[SerializeField] private Button localHostButton;
    //[SerializeField] private Button localConnectButton;
    [SerializeField] private Button mainMenuButton;


    private void Awake() {
        Instance = this;

        //helpButton.onClick.AddListener(() => {

        //});

        //lobbyButton.onClick.AddListener(() => {

        //});

        hostButton.onClick.AddListener(() => {
            Hide();
            MultiplayerMenuUI.Instance.Show();
        });

        connectButton.onClick.AddListener(() => {
            Hide();
            MultiplayerMenuUI.Instance.Show();
        });

        //localHostButton.onClick.AddListener(() => {

        //});

        //localConnectButton.onClick.AddListener(() => {

        //});

        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
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
