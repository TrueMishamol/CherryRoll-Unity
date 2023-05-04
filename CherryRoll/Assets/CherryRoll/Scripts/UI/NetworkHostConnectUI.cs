using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkHostConnectUI : MonoBehaviour {

    [SerializeField] private Button hostButton;
    [SerializeField] private Button connectButton;
    [SerializeField] private TextMeshProUGUI joinCodeInputField;
    [SerializeField] private TextMeshProUGUI nameInputField;

    //private string playerName;

    private void Awake() {
        hostButton.onClick.AddListener(() => {
            NetworkHandleConnection.Instance.CreateRelay();

            SwitchUIToDisconnectUI();
            UpdateTheNameFromField();
        });

        connectButton.onClick.AddListener(() => {
            string joinCode;
            joinCode = joinCodeInputField.text.ToUpper();
            joinCode = joinCode.Remove(joinCode.Length - 1);  // Convert from TMPro. TMPro adds an invisible character at the end
            NetworkHandleConnection.Instance.UpdateJoinCode(joinCode);

            NetworkHandleConnection.Instance.JoinRelay();

            SwitchUIToDisconnectUI();
            UpdateTheNameFromField();
        });
    }

    private void SwitchUIToDisconnectUI() {
        Hide();
        NetworkDisconnectUI.Instance.Show();
        IngameMenuUI.Instance.Hide();
    }

    private void UpdateTheNameFromField() {
        //if (!IsOwner) return;
        //Конвертация из TMPro. Он добавляет невидимый символ в конце
        //playerName = nameChangeInputField.text;
        //playerName = playerName.Remove(playerName.Length - 1);

        //string playerName;
        //playerName = nameInputField.text.Remove(playerName.Length - 1);

        //playerName = nameInputField.text;
        //Debug.Log(playerName);
        //PlayerName.LocalInstance.ChangePlayerName(playerName);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
