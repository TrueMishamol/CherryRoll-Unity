using Unity.Netcode;
using UnityEngine;

public class MainMenuCleanUp : MonoBehaviour {

    private void Awake() {
        if (NetworkManager.Singleton != null) {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if (Multiplayer.Instance != null) {
            Destroy(Multiplayer.Instance.gameObject);
        }
    }
}
