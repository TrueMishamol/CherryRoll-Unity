using UnityEngine;
using UnityEngine.UI;

public class GameChooseMenuUI : BaseMenuUI {


    public static GameChooseMenuUI Instance { get; private set; }

    [SerializeField] private Button closeButton;

    [SerializeField] private Transform container;
    [SerializeField] private Transform gameTemplate;
    [SerializeField] private GameSceneSOListSO gameSceneSOListSO;


    private void Awake() {
        Instance = this;

        gameTemplate.gameObject.SetActive(false);

        closeButton.onClick.AddListener(() => {
            Close();
        });
    }

    private void Start() {
        foreach (GameSceneSO gameSceneSO in gameSceneSOListSO.gameSceneSOList) {
            Transform gameTemplateTransform = Instantiate(gameTemplate, container);
            gameTemplateTransform.gameObject.SetActive(true);
            gameTemplateTransform.GetComponent<GameChooseMenuSingleUI>().SetGameSceneSO(gameSceneSO);
        }

        Close();
    }
}
