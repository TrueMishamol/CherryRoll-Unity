using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : NetworkBehaviour
{
    //private GameObject playerName;
    //private GameInput gameInput;
    private GameObject playerNameTextFieldGameObject;
    private GameObject changeNameButtonGameObject;
    private string playerNameTextFieldName = "PlayerNameTextField";
    private string changeNameButtonName = "ChangeNameButton";
    private TextMeshProUGUI playerNameTextFieldTMP;
    private Button changeNameButtonButton;

    [SerializeField] private TextMeshProUGUI playerDisplayName;

    private NetworkVariable<FixedString128Bytes> playerName = new NetworkVariable<FixedString128Bytes>
        ("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        NameChange(playerName.Value.ToString());

        playerName.OnValueChanged += (FixedString128Bytes previousValue, FixedString128Bytes newValue) =>
        {
            NameChange(playerName.Value.ToString());
            Debug.Log("Player " + previousValue + " change name to " + newValue);
        };

        // -------------------------

        //playerName = playerNameTextFieldTMP.text;

        //string playerEnterNameString = playerNameTextFieldTMP.text;
        //playerEnterNameString = playerEnterNameString.Remove(playerEnterNameString.Length - 1);
        //FixedString128Bytes.TryParse(playerEnterNameString, out playerName);

        //playerName = playerEnterNameString;

        //playerName = new FixedString128Bytes(;

        //FixedString128Bytes.
        //string.Format()

        //if (playerEnt)
        //    playerName.Value = "Bun " + OwnerClientId;
        //playerDisplayName.text = playerName.Value.ToString();
        //meshRenderer.material.color = new Color(0, 0, 0);
    }

    private void Awake()
    {
        //changeNameButtonButton.onClick.AddListener(() =>
        //{
        //    NameChange();
        //});
    }

    private void Start()
    {


        if (IsOwner)
        {
            playerNameTextFieldGameObject = GameObject.Find(playerNameTextFieldName);
            changeNameButtonGameObject = GameObject.Find(changeNameButtonName);

            //    playerNameTextFieldTMP = playerNameTextFieldGameObject;
            //    changeNameButtonButton = changeNameButtonGameObject;
        }
    }

    private void NameChange(string newPlayerName)
    {
        playerName.Value = newPlayerName;
        
        if (OwnerClientId == 0 & playerName.Value == "") 
            playerName.Value = "Baker";

        if (playerName.Value == "")
            playerName.Value = "Bun " + OwnerClientId;

        playerDisplayName.text = playerName.Value.ToString();
    }

}
