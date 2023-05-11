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
    }

    private void Start() {
        //! Originally events are listening on start, but also on the start we ran IngameMenuUI.Hide()
        Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;

        Hide();
    }

    private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e) {
        if (MultiplayerPlayersCount.Instance.GetPlayersCount() == 0) {
            playersCountText.text = "Server stopped";
        } else {
            playersCountText.text = "Players: " + MultiplayerPlayersCount.Instance.GetPlayersCount().ToString();
        }
        //! Is it working?
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
