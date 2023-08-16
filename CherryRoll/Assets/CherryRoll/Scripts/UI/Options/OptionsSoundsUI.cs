using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsSoundsUI : MonoBehaviour {


    public static OptionsSoundsUI Instance { get; private set; }


    [SerializeField] private Button closeButton;

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;


    private void Awake() {
        Instance = this;

        closeButton.onClick.AddListener(() => {
            Hide();
        });

        //soundEffectsButton.onClick.AddListener(() => {
        //    SoundManager.Instance.ChangeVolume();
        //    UpdateVisual();
        //});

        //musicButton.onClick.AddListener(() => {
        //    MusicManager.Instance.ChangeVolume();
        //    UpdateVisual();
        //});
    }

    private void Start() {
        UpdateVisual();

        Hide();
    }

    private void UpdateVisual() {
        //soundEffectsText.text = "Sound effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        //musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
