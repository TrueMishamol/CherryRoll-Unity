using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour {

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
