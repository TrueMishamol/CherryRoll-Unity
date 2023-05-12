using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameMenuUI : MonoBehaviour {


    public static IngameMenuUI Instance { get; private set; }

    [SerializeField] private Button joinCodeCopyButton;
    [SerializeField] private TextMeshProUGUI joinCodeOutputText;
    [SerializeField] private Button optionsButton;
    [SerializeField] private TextMeshProUGUI nameInputField;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI playersCountNumberText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button backButton;

    private bool isIngameMenuOppened = false;


    private void Awake() {
        Instance = this;

        joinCodeCopyButton.onClick.AddListener(() => {
            CopyToClipboard.Copy(MultiplayerConnection.JoinCode);
        });

        optionsButton.onClick.AddListener(() => {

        });

        quitButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.LocalClientId == NetworkManager.ServerClientId) {
                // Is Host
                if (SceneManager.GetActiveScene().ToString() != Loader.Scene.LobbyScene.ToString()) {
                    // Active scene is NOT Lobby
                    Loader.LoadNetwork(Loader.Scene.LobbyScene);
                } else {
                    // Active scene is Lobby
                    Loader.Load(Loader.Scene.MainMenuScene);
                }
            } else {
                // Is Client
                Loader.Load(Loader.Scene.MainMenuScene);
            }
        });

        closeButton.onClick.AddListener(() => {
            SwitchOpenClose();
            PauseGameManager.Instance.ToggleLocalPauseGame();
        });

        backButton.onClick.AddListener(() => {

        });

        //DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        GameInput.Instance.OnMenuOpenCloseAction += GameInput_OnMenuOpenCloseAction;
        MultiplayerConnection.OnJoinCodeUpdated += NetworkHandleConnection_OnJoinCodeUpdated;
        Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;

        UpdateJoinCodeOutputText();
        UpdatePlayersCountOutputTextServerRpc();

        Hide();
    }

    private void NetworkHandleConnection_OnJoinCodeUpdated(object sender, EventArgs e) {
        UpdateJoinCodeOutputText();
    }

    private void UpdateJoinCodeOutputText() {
        joinCodeOutputText.text = MultiplayerConnection.JoinCode;
    }

    private void GameInput_OnMenuOpenCloseAction(object sender, System.EventArgs e) {
        SwitchOpenClose();
    }

    private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e) {
        UpdatePlayersCountOutputTextServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePlayersCountOutputTextServerRpc() {
        UpdatePlayersCountOutputTextClientRpc();
    }

    [ClientRpc]
    private void UpdatePlayersCountOutputTextClientRpc() {
        playersCountNumberText.text = MultiplayerPlayersCount.Instance.GetPlayersCount().ToString();
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
