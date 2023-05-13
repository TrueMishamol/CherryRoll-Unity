using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour {


    [SerializeField] private Button mainMenuButton;
    private float countdownToShowMainMenuButton = 7f;


    private void Awake() {
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start() {
        mainMenuButton.gameObject.SetActive(false);
    }

    private void Update() {
        if (mainMenuButton.gameObject.activeSelf) return;

        countdownToShowMainMenuButton -= Time.deltaTime;
        if (countdownToShowMainMenuButton < 0f) {
            mainMenuButton.gameObject.SetActive(true);
        }
    }

}
