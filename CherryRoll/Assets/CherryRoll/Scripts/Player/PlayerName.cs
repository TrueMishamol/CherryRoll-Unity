using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;
using UnityEngine.UI;

public class PlayerName : NetworkBehaviour
{
    //private GameInput gameInput;

    [SerializeField] private TextMeshProUGUI playerDisplayName;

    private NetworkVariable<FixedString128Bytes> playerName = new NetworkVariable<FixedString128Bytes>
        ("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        NameChange(playerName.Value.ToString());

        playerName.OnValueChanged += (FixedString128Bytes previousValue, FixedString128Bytes newValue) =>
        {
            NameChange(playerName.Value.ToString());
        };
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
