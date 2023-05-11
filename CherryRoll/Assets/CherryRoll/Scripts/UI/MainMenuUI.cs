using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {


    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI gameVersionText;


    private void Awake() {
        playButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MultiplayerMenuScene);
        });

        quitButton.onClick.AddListener(() => {
            Debug.Log("Quit");
            Application.Quit();
        });

        Time.timeScale = 1f;
    }

    private void Start() {
        gameVersionText.text = Application.version;
    }
}
