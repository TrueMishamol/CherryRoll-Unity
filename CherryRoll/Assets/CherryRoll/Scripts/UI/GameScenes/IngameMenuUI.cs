using UnityEngine;

public class IngameMenuUI : MonoBehaviour {


    public static IngameMenuUI Instance { get; private set; }


    private void Awake() {
        //if (Instance != null & Instance != this) {
        //    Destroy(Instance.gameObject);
        //}
        Instance = this;

        //DontDestroyOnLoad(gameObject);
    }
}
