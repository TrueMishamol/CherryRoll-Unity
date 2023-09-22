using Cinemachine;
using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void FollowPlayer(Transform personToFollow)
    {
        cinemachineVirtualCamera.Follow = personToFollow;
        cinemachineVirtualCamera.LookAt = personToFollow;
    }
}
