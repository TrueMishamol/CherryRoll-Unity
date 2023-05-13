using Unity.Netcode;
using UnityEngine;

public class MainMenuCleanUp : MonoBehaviour {

    // Cleans up all objects with DontDestroyOnLoad(gameObject);

    private void Awake() {
        if (NetworkManager.Singleton != null) {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if (Multiplayer.Instance != null) {
            Destroy(Multiplayer.Instance.gameObject);
        }

        if (MultiplayerConnection.Instance != null) {
            Destroy(MultiplayerConnection.Instance.gameObject);
        }

        if (MultiplayerPlayersCount.Instance != null) {
            Destroy(MultiplayerPlayersCount.Instance.gameObject);
        }

        if (IngameMenuUI.Instance != null) {
            Destroy(IngameMenuUI.Instance.gameObject);
        }

        if (GameInput.Instance != null) {
            Destroy(GameInput.Instance.gameObject);
        }

        if (GamePause.Instance != null) {
            Destroy(GamePause.Instance.gameObject);
        }
    }
}
