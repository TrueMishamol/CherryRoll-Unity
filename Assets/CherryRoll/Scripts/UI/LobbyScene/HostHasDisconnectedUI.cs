using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostHasDisconnectedUI : MonoBehaviour {


    [SerializeField] private Button mainMenuButton;


    private void Awake() {
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MenuMainMenuScene);
        });
    }

    private void Start() {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;

        Hide();
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId) {
        if (clientId == NetworkManager.ServerClientId) {
            //^ Server is shutting down
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
