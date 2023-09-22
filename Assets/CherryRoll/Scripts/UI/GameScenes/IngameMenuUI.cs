using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameMenuUI : BaseMenuUI {


    public static IngameMenuUI Instance;

    public event EventHandler OnMenuOpened;
    public event EventHandler OnMenuClosed;

    [SerializeField] private Button joinCodeCopyButton;
    [SerializeField] private TextMeshProUGUI joinCodeOutputText;
    [SerializeField] private Button optionsButton;
    [SerializeField] private TextMeshProUGUI nameInputField;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI playersCountNumberText;
    [SerializeField] private Button closeButton;


    private void Awake() {
        Instance = this;

        joinCodeCopyButton.onClick.AddListener(() => {
            CopyToClipboard.Copy(MultiplayerConnection.JoinCode);
        });

        optionsButton.onClick.AddListener(() => {

        });

        quitButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.LocalClientId == NetworkManager.ServerClientId) {
                //^ Is Host
                if (SceneManager.GetActiveScene().name.ToString() != Loader.Scene.GameLobbyScene.ToString()) {
                    //^ Active scene is NOT Lobby
                    Loader.LoadNetwork(Loader.Scene.GameLobbyScene);
                } else {
                    //^ Active scene is Lobby
                    NetworkManager.Singleton.Shutdown();
                    Loader.Load(Loader.Scene.MenuMainMenuScene);
                }
            } else {
                //^ Is Client
                NetworkManager.Singleton.Shutdown();
                Loader.Load(Loader.Scene.MenuMainMenuScene);
            }

            Close();
        });

        closeButton.onClick.AddListener(() => {
            SwitchOpenClose();
        });
    }

    private void Start() {
        MultiplayerConnection.OnJoinCodeUpdated += NetworkHandleConnection_OnJoinCodeUpdated;
        MultiplayerPlayersCount.OnPlayerCountUpdate += MultiplayerPlayersCount_OnPlayerCountUpdate;

        UpdateJoinCodeOutputText();
        UpdatePlayersCountOutputTextServerRpc();

        Close();
    }

    private void MultiplayerPlayersCount_OnPlayerCountUpdate(object sender, EventArgs e) {
        UpdatePlayersCountOutputTextServerRpc();
    }

    private void NetworkHandleConnection_OnJoinCodeUpdated(object sender, EventArgs e) {
        UpdateJoinCodeOutputText();
    }

    private void UpdateJoinCodeOutputText() {
        joinCodeOutputText.text = MultiplayerConnection.JoinCode;
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePlayersCountOutputTextServerRpc() {
        UpdatePlayersCountOutputTextClientRpc();
    }

    [ClientRpc]
    private void UpdatePlayersCountOutputTextClientRpc() {
        playersCountNumberText.text = MultiplayerPlayersCount.Instance.GetPlayersCount().ToString();
    }

    protected override void Close() {
        base.Close();
        GamePause.Instance.SetLocalGamePaused(isOppened);
    }

    protected override void Open() {
        base.Open();
        GamePause.Instance.SetLocalGamePaused(isOppened);
    }
}
