using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] float spawnPositionRange = 5f;

    [SerializeField] private NetworkVariable<Color> playerColor = new NetworkVariable<Color>(new Color(1, 1, 1), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private MeshRenderer meshRenderer;

    // Interaction
    [SerializeField] private LayerMask interactLayerMask;
    private Vector3 lastInteractDir;
    private Wardrobe selectedWarderobe;

    // Game input
    private GameObject gameInputHolder;
    private GameInput gameInput;
    private string gameInputHolderName = "GameInput";

    // Cinemachine
    [SerializeField] private PlayerCameraFollow playerCameraFollow;
    private GameObject cameraFollow;
    private string cameraName = "CameraFollow";

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        if (IsOwner)
        {
            // Game Input
            gameInputHolder = GameObject.Find(gameInputHolderName);
            gameInputHolder.TryGetComponent<GameInput>(out gameInput);
        }

        gameInput.OnInteractAction += GameInput_OnInteractAction;

        if (IsClient && IsOwner)
        {
            // Cinemachine
            cameraFollow = GameObject.Find(cameraName);
            cameraFollow.TryGetComponent<PlayerCameraFollow>(out playerCameraFollow);
            playerCameraFollow.FollowPlayer(transform);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        Vector2 inputVector = gameInput.GetMovementVectorSmoothed();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = .55f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, interactLayerMask))
        {
            // Wardrobe
            if (raycastHit.transform.TryGetComponent(out Wardrobe wardrobe))
            {
                playerColor.Value = wardrobe.RandomizePlayerColor();
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        // Update Player Color
        playerColor.OnValueChanged += (Color previousValue, Color newValue) =>
        {
            meshRenderer.material.color = newValue;
            Debug.Log("Player " + OwnerClientId + " change color from " + previousValue + " to " + newValue);
        };

        // See others' Players Colors on Server Join
        meshRenderer.material.color = playerColor.Value;

        if (!IsOwner) return;
        RandomSpawnServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RandomSpawnServerRpc()
    {
        // ? Idea: Spawn Players near Host or Object
        transform.position = new Vector3(Random.Range(spawnPositionRange, -spawnPositionRange), 0, Random.Range(spawnPositionRange, -spawnPositionRange));

        // Rotates player to face Camera
        transform.rotation = new Quaternion(0, 180, 0, 0);
    }

    private void Update()
    {
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorSmoothed();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = .55f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, interactLayerMask))
        {
            // Wardrobe
            if (raycastHit.transform.TryGetComponent(out Wardrobe wardrobe))
            {
                playerColor.Value = wardrobe.RandomizePlayerColor();
            }
        }
    }
}
