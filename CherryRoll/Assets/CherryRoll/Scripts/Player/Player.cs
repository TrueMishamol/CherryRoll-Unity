using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour {

    public static event EventHandler OnAnyPlayerSpawned;

    public static void ResetStaticData() {
        OnAnyPlayerSpawned = null;
    }

    public static Player LocalInstance { get; private set; }

    public event EventHandler<OnSelectedInteractableObjectChangedEventArgs> OnSelectedInteractableObjectChanged;
    public class OnSelectedInteractableObjectChangedEventArgs : EventArgs {
        public BaseInteractableObject selectedInteractableObject;
    }

    [SerializeField] float spawnPositionRange = 5f;

    [SerializeField] private NetworkVariable<Color> playerColor = new NetworkVariable<Color>(new Color(1, 1, 1), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private MeshRenderer meshRenderer;

    // Interaction
    [SerializeField] private LayerMask interactLayerMask;
    private Vector3 lastInteractDir;

    private BaseInteractableObject selectedInteractableObject;

    // Cinemachine
    [SerializeField] private PlayerCameraFollow playerCameraFollow;
    private GameObject cameraFollow;
    private string cameraName = "CameraFollow";

    private void Awake() {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start() {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

        if (IsClient && IsOwner) {
            // Cinemachine
            cameraFollow = GameObject.Find(cameraName);
            cameraFollow.TryGetComponent<PlayerCameraFollow>(out playerCameraFollow);
            playerCameraFollow.FollowPlayer(transform);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {

        if (selectedInteractableObject != null) {
            selectedInteractableObject.Interact(this);
        }
    }

    public void ChangePlayerColor(Color playerColor) {
        this.playerColor.Value = playerColor;
    }

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            LocalInstance = this;

            RandomSpawnServerRpc();
        }

        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);

        // Update Player Color
        playerColor.OnValueChanged += (Color previousValue, Color newValue) => {
            meshRenderer.material.color = newValue;
            Debug.Log("Player " + OwnerClientId + " change color from " + previousValue + " to " + newValue);
        };

        // See others' Players Colors on Server Join
        meshRenderer.material.color = playerColor.Value;
    }

    [ServerRpc(RequireOwnership = false)]
    private void RandomSpawnServerRpc() {
        transform.position = new Vector3(UnityEngine.Random.Range(spawnPositionRange, -spawnPositionRange), 0, UnityEngine.Random.Range(spawnPositionRange, -spawnPositionRange));

        // Rotates player to face Camera
        transform.rotation = new Quaternion(0, 180, 0, 0);
    }

    private void Update() {

        if (!IsOwner) return;
        HandleInteractions();
    }

    private void HandleInteractions() {

        Vector2 inputVector = GameInput.Instance.GetMovementVectorSmoothed();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        float interactDistance = .55f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, interactLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out BaseInteractableObject baseInteractableObject)) {
                // Has BaseInteractableObject
                if (baseInteractableObject != selectedInteractableObject) {
                    SetSelectedInteractableObject(baseInteractableObject);
                }
            } else {
                // Has no BaseInteractableObject
                SetSelectedInteractableObject(null);
            }
        } else {
            // Raycast do not hit interactLayerMask
            SetSelectedInteractableObject(null);
        }
    }

    private void SetSelectedInteractableObject(BaseInteractableObject selectedInteractableObject) {
        this.selectedInteractableObject = selectedInteractableObject;

        OnSelectedInteractableObjectChanged?.Invoke(this, new OnSelectedInteractableObjectChangedEventArgs {
            selectedInteractableObject = selectedInteractableObject
        });
    }
}

