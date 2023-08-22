using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI gameVersionText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button mishamolLogoButton;
    [SerializeField] private Button debugButton;


    private void Awake() {
        playButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MenuMultiplayerScene);
        });

        quitButton.onClick.AddListener(() => {
            Debug.Log("Quit");
            Application.Quit();
        });

        Time.timeScale = 1f;

        mishamolLogoButton.onClick.AddListener(() =>
        {
            //! Add a question alert-window "Are you shure you want to open link in external application / browser?"
            Application.OpenURL("https://mishamol.ru/");
        });

        debugButton.onClick.AddListener(() =>
        {
            //DebugUI.Instance.SwitchOpenClose();
            DebugConsoleSwitcher.Instance.SwitchOpenClose();
        });
    }

    private void Start() {
        gameVersionText.text = Application.version;
    }
}
