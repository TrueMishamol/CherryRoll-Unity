using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerSceneSwitcher : NetworkBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //NetworkSceneManager.
            //NetworkManager.ServerChangeScene("PlayScene");
            //NetworkManager.Singleton.StartHost();
        };
    }
}
