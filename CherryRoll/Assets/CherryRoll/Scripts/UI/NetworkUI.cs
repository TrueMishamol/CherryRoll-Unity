using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : NetworkBehaviour {


    public static NetworkUI Instance { get; private set; }

    public static string JoinCode;


    [SerializeField] private Button backButton;


    private void Awake() {
        Instance = this;

        backButton.onClick.AddListener(() => {
            Hide();
            MainIngameMenuUI.Instance.Show();
        });
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
