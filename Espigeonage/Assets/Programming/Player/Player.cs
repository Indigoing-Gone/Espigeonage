using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCamera))]
[RequireComponent(typeof(Grabber))]
[RequireComponent(typeof(RaycastInteractor))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform cameraOrientation;

    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;
    private Grabber playerGrab;
    private RaycastInteractor playerInteract;

    private void OnEnable()
    {
        playerInput.InteractStatus += playerInteract.OnInteractStatus;
    }

    private void OnDisable()
    {
        playerInput.InteractStatus -= playerInteract.OnInteractStatus;
    }

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCamera = GetComponent<PlayerCamera>();
        playerGrab = GetComponent<Grabber>();
        playerInteract = GetComponent<RaycastInteractor>();

        playerMovement.Orientation = cameraOrientation;
        playerCamera.Orientation = cameraOrientation;
        playerInteract.Orientation = cameraOrientation;
    }

    private void Update()
    {
        playerMovement.ProcessInput(playerInput.MoveDirection);
        playerCamera.ProcessInput(playerInput.LookDirection);
    }
}
