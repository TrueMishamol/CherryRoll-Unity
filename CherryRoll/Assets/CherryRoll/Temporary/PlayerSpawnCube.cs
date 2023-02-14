using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnCube : NetworkBehaviour
{
    [SerializeField] private Transform spawnCubePrefab;

    private Transform spawnedObjectTransform;

    private void Update()
    {
        if (!IsOwner) return;

        //Спавнит объект
        if (Input.GetKeyDown(KeyCode.C))
        {
            ObjectSpawnServerRpc();
        };
        if (Input.GetKeyDown(KeyCode.X))
        {
            ObjectDeleteServerRpc();
        };
    }

    //Игрок создаёт перед собой куб или пр.
    [ServerRpc]
    private void ObjectSpawnServerRpc() //! string objectType //! Так же надо записывать координаты где он создаёт объект
    {
        spawnedObjectTransform = Instantiate(spawnCubePrefab);
        spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
    }

    [ServerRpc]
    private void ObjectDeleteServerRpc() //! Должен деспаунить все или в обратном порядке. Деспунит только последний
    {
        Destroy(spawnedObjectTransform.gameObject);
    }
}
