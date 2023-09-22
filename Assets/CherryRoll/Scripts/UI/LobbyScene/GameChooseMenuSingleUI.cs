using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameChooseMenuSingleUI : NetworkBehaviour {


    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Transform image;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Button playButton;


    public void SetGameSceneSO(GameSceneSO recipeSO) {
        title.text = recipeSO.gameName;
        image.GetComponent<Image>().sprite = recipeSO.gamePreview;
        description.text = recipeSO.gameDescription;

        if (NetworkManager.IsHost) {
            playButton.onClick.AddListener(() => {
                Loader.LoadNetwork(recipeSO.gameScene);
            });
        } else {
            playButton.gameObject.SetActive(false);
        }
    }
}
