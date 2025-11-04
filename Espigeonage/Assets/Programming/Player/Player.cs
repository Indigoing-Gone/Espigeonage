using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FirstPersonMovement))]
[RequireComponent(typeof(FirstPersonCamera))]
[RequireComponent(typeof(RaycastInteractor))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CinemachineCamera cam;
    public CinemachineCamera Cam => cam;
    [SerializeField] private Transform cameraOrientation;

    private FirstPersonMovement playerMovement;
    private FirstPersonCamera playerCamera;
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
        playerMovement = GetComponent<FirstPersonMovement>();
        playerCamera = GetComponent<FirstPersonCamera>();
        playerInteract = GetComponent<RaycastInteractor>();

        playerMovement.Orientation = cameraOrientation;
        playerCamera.Orientation = cameraOrientation;
        playerInteract.Origin = cameraOrientation;
    }

    private void Update()
    {
        playerMovement.ProcessInput(playerInput.MoveDirection);
        playerCamera.ProcessInput(playerInput.LookDirection);
    }
}
