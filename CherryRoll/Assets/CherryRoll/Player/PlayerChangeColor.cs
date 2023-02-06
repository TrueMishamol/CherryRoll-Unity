using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChangeColor : NetworkBehaviour
{
    private MeshRenderer meshRenderer;
    private NetworkVariable<Color> randomColor = new NetworkVariable<Color>(new Color(1, 1, 1), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Update()
    {
        //Весь последующий скрипт работает только если ты Owner
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Y))
        {
            //Рандомизирует значения
            randomColor.Value = new Color(Random.value, Random.value, Random.value);
        };
    }

    public override void OnNetworkSpawn()
    {
        //Изменяет цвет игрока
        randomColor.OnValueChanged += (Color previousValue, Color newValue) =>
        {
            meshRenderer.material.color = newValue;
            Debug.Log("Player " + OwnerClientId + " color set to " + newValue);
        };

        //Чтобы видеть чужие уже выбранные цвета при подключении
        meshRenderer.material.color = randomColor.Value;
    }
}
