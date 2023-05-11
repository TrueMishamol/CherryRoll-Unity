using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkDisconnectUI : MonoBehaviour {

    public static NetworkDisconnectUI Instance { get; private set; }

    [Header("Second Screen Elements (menuDisconnect)")]
    [SerializeField] private Button disconnectButton;
    [SerializeField] private TextMeshProUGUI nameChangeInputField;
    [SerializeField] private Button nameChangeButton;
    [SerializeField] private TextMeshProUGUI playersCountText;

    private void Awake() {
        Instance = this;

        // Originally events are listening on start, but also on the start we ran IngameMenuUI.Hide()
        MultiplayerPlayersCount.OnPlayersCountUpdated += NetworkHandleConnection_OnPlayersCountUpdated;
    }

    private void Start() {
        Hide();
    }

    //! Refactor
    private void Update() {
        if (MultiplayerPlayersCount.Instance.GetPlayersCount() == 0) {
            playersCountText.text = "Server stopped";
        } else {
            playersCountText.text = "Players: " + MultiplayerPlayersCount.Instance.GetPlayersCount().ToString();
        }
    }

    private void NetworkHandleConnection_OnPlayersCountUpdated(object sender, System.EventArgs e) {
        if (MultiplayerPlayersCount.Instance.GetPlayersCount() == 0) {
            playersCountText.text = "Server stopped";
        } else {
            playersCountText.text = "Players: " + MultiplayerPlayersCount.Instance.GetPlayersCount().ToString();
        }
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
