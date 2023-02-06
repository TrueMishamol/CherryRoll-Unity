using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;
using UnityEngine.UI;

public class PlayerSettings : NetworkBehaviour
{
    //[SerializeField] private TextMeshProUGUI playerEnterName;
    [SerializeField] private TextMeshProUGUI playerDisplayName;
    //[SerializeField] private Button enterNameButton;
    public NetworkVariable<FixedString128Bytes> playerName = new NetworkVariable<FixedString128Bytes>
        ("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        NameChange(playerName.Value.ToString());

        playerName.OnValueChanged += (FixedString128Bytes previousValue, FixedString128Bytes newValue) =>
        {
            NameChange(playerName.Value.ToString());
            Debug.Log("Player " + previousValue + " changed name to " + newValue);
        };
    }

    //private void Awake()
    //{
    //    enterNameButton.onClick.AddListener(() => {
    //        NameChange();
    //    });
    //}

    public void NameChange(string newPlayerName)
    {
        playerName.Value = newPlayerName;
        
        if (OwnerClientId == 0 & playerName.Value == "") 
            playerName.Value = "Baker";

        if (playerName.Value == "")
            playerName.Value = "Bun " + OwnerClientId;

        playerDisplayName.text = playerName.Value.ToString();
    }

    //public override void OnNetworkSpawn()
    //{
    //    playerName = playerEnterName.text;

    //    string playerEnterNameString = playerEnterName.text;
    //    playerEnterNameString = playerEnterNameString.Remove(playerEnterNameString.Length - 1);
    //    FixedString128Bytes.TryParse(playerEnterNameString, out playerName);

    //    playerName = playerEnterNameString;

    //    playerName = new FixedString128Bytes(;

    //    FixedString128Bytes.
    //    string.Format()

    //    if (playerEnt)
    //        playerName.Value = "Bun " + OwnerClientId;
    //    playerDisplayName.text = playerName.Value.ToString();
    //    //meshRenderer.material.color = new Color(0,0,0);
    //}
}
