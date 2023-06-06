using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI gameVersionText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;


    private void Awake() {
        playButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MultiplayerMenuScene);
        });

        optionsButton.onClick.AddListener(() => {
            OptionsMainUI.Instance.Show();
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
