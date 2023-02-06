using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] float playerSpeed = 3f;
    [SerializeField] float playerRotationSpeed = 500f;
    [SerializeField] float positionRange = 5f;

    //Input System
    Vector2 moveInput;
    Vector3 moveDir = new Vector3(0, 0, 0);

    private Rigidbody playerRigidbody;
    private PlayerInputActions playerInputActions;
    private PlayerInput playerInput;
    //!private Animator animator;

    private void Awake()
    {
        //!animator = GetComponent<Animator>();

        //Инициализация всех переменных и движений для Input System
        playerRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.performed += Jump;
    }

    private void Update()
    {
        //Весь последующий скрипт работает только если ты Owner
        if (!IsOwner) return;

        Walk();
    }

    public void Walk()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        //Перемещает transform в направлении движения
        //transform.Translate(movementDirection * playerSpeed * Time.deltaTime, Space.World);

        //Поворачивает игрока в направлении движения
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, playerRotationSpeed * Time.deltaTime);
        }

        //!Меняет анимацию на бег
        //animator.SetFloat("run", movementDirection.magnitude);
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        RandomSpawnServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RandomSpawnServerRpc()
    {
        //Рандомный радиус спауна. Поворот игрока к камере
        //! Тут можно попробоапть спаунить игроков у сферы или рядом с хостом, чтобы они не терялись
        transform.position = new Vector3(Random.Range(positionRange, -positionRange), 0, Random.Range(positionRange, -positionRange));
        transform.rotation = new Quaternion(0, 180, 0, 0);
    }

    //INPUT SYSTEM УПРАВЛЕНИЕ

    //Бег
    private void FixedUpdate()
    {
        Run();
    }
    public void Run()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        playerRigidbody.transform.position += (new Vector3(inputVector.x, 0, inputVector.y) * playerSpeed * Time.deltaTime);
        //! Игрок резко тормозит когда отпускаешь движение, потомучто у него нет инерции. Добавить
        //playerRigidbody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * playerSpeedForce, ForceMode.Impulse);
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    //Прыжки
    public void Jump(InputAction.CallbackContext context)
    {
        playerRigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }

}
