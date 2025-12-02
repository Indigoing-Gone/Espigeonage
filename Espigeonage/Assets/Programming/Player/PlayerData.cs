using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(FirstPersonMovement))]
[RequireComponent(typeof(FirstPersonCamera))]
[RequireComponent(typeof(CameraSwitcher))]

[RequireComponent(typeof(RaycastInteractor))]
[RequireComponent(typeof(InspectGrabber))]
[RequireComponent(typeof(Dragger))]
[RequireComponent(typeof(Drawer))]

[RequireComponent(typeof(PlayerUI))]
public class PlayerData : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputReader input;
    public InputReader Input => input;

    [Header("Input Variables")]
    public Vector2 MoveDirection { get; private set; }
    public Vector2 LookDirection { get; private set; }
    public Vector3 MousePosition { get; private set; }

    [Header("Camera")]
    [SerializeField] private CinemachineCamera playerCamera;
    public CinemachineCamera PlayerCamera => playerCamera;
    [SerializeField] private Transform cameraOrientation;
    public Transform CameraOrientation => cameraOrientation;

    [Header("Player Components")]
    public FirstPersonMovement MovementComponent { get; private set; }
    public FirstPersonCamera CameraComponent { get; private set; }
    public CameraSwitcher CameraSwitcher { get; private set; }

    public RaycastInteractor Interactor { get; private set; }
    public InspectGrabber Grabber { get; private set; }
    public Dragger Dragger { get; private set; }
    public Drawer Drawer { get; private set; }

    public PlayerUI PlayerUI { get; private set; }

    [Header("Desk")]
    public Desk CurrentDesk { get; private set; }
    public bool AtDesk => CurrentDesk != null;

    private void OnEnable()
    {
        input.MoveEvent += ProcessMove;
        input.LookEvent += ProcessLook;
        input.PositionEvent += ProcessPosition;
        input.InteractEvent += Interact;
        input.CycleEvent += Drawer.CycleBrush;

        Interactor.TargetInteractableUpdated += UpdateTooltip;
    }

    private void OnDisable()
    {
        input.MoveEvent -= ProcessMove;
        input.LookEvent -= ProcessLook;
        input.PositionEvent -= ProcessPosition;
        input.InteractEvent -= Interact;
        input.CycleEvent -= Drawer.CycleBrush;

        Interactor.TargetInteractableUpdated -= UpdateTooltip;
    }

    private void Awake()
    {
        MovementComponent = GetComponent<FirstPersonMovement>();
        CameraComponent = GetComponent<FirstPersonCamera>();
        CameraSwitcher = GetComponent<CameraSwitcher>();

        Interactor = GetComponent<RaycastInteractor>();
        Grabber = GetComponent<InspectGrabber>();
        Dragger = GetComponent<Dragger>();
        Drawer = GetComponent<Drawer>();

        PlayerUI = GetComponent<PlayerUI>();

        MovementComponent.Orientation = cameraOrientation;
        CameraComponent.Orientation = cameraOrientation;

        PlayerUI.SetDefaultUI();
    }

    private void ProcessMove(Vector2 _direction) => MoveDirection = _direction; 
    private void ProcessLook(Vector2 _direction) => LookDirection = _direction;
    private void ProcessPosition(Vector2 _position) => MousePosition = _position;

    private void Interact(bool _state) { if (_state) Interactor.TriggerInteraction(); }
    public void ReleaseDrag(bool _state) { if (!_state) Dragger.Release(); }
    public void StopDrawing(bool _state) { if (!_state) Drawer.StopDrawing(); }

    private void UpdateTooltip(IInteractable _interactable, InteractionData _interaction)
    {
        if (_interactable == null || _interaction.behaviour == null)
        {
            PlayerUI.SetDefaultUI();
            return;
        }

        PlayerUI.ApplyDisplayData(_interaction);
    }

    public void SetCurrentDesk(Desk _newDesk) => CurrentDesk = _newDesk;
    public void ExitDesk() => SetCurrentDesk(null);
}
