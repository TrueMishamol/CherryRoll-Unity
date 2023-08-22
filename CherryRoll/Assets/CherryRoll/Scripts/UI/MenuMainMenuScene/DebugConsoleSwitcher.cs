using UnityEngine;

public class DebugConsoleSwitcher : MonoBehaviour {


    private const string DEBUG_CONSOLE_GAMEOBJECT_NAME = "/IngameDebugConsole";

    public static DebugConsoleSwitcher Instance;

    private static bool isOppened = false;
    private GameObject ingameDebugConsole;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        ingameDebugConsole = GameObject.Find(DEBUG_CONSOLE_GAMEOBJECT_NAME);

        UpdateOpenClose();
    }

    private void Show() {
        isOppened = true;
        ingameDebugConsole.GetComponent<Canvas>().enabled = true;
    }

    private void Hide() {
        isOppened = false;
        ingameDebugConsole.GetComponent<Canvas>().enabled = false;
    }

    public void SwitchOpenClose() {
        isOppened = !isOppened;

        UpdateOpenClose();
    }

    private void UpdateOpenClose() {
        if (isOppened) {
            Show();
        } else {
            Hide();
        }
    }
}
