using Unity.Cinemachine;
using UnityEngine;

public enum GameState
{
    None = 0,
    Movement = 1,
    Desk = 2,
}

public class PlayerStateMachine : MonoBehaviour
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
    public Grabber Grabber { get; private set; }
    public Dragger Dragger { get; private set; }

    [Header("States")]
    private GameState currentStateEnum;

    private PlayerStateBase currentState;
    public MovementPlayerState MovementState { get; private set; }
    public DeskPlayerState DeskState { get; private set; }

    private void OnEnable()
    {
        input.MoveEvent += HandleMove;
        input.LookEvent += HandleLook;
        input.PositionEvent += HandlePosition;
        input.InteractEvent += Interactor.OnInteractStatus;
    }

    private void OnDisable()
    {
        input.MoveEvent -= HandleMove;
        input.LookEvent -= HandleLook;
        input.PositionEvent -= HandlePosition;
        input.InteractEvent -= Interactor.OnInteractStatus;
    }

    private void Awake()
    {
        MovementComponent = GetComponent<FirstPersonMovement>();
        CameraComponent = GetComponent<FirstPersonCamera>();
        CameraSwitcher = GetComponent<CameraSwitcher>();
        Interactor = GetComponent<RaycastInteractor>();
        Grabber = GetComponent<Grabber>();
        Dragger = GetComponent<Dragger>();

        currentStateEnum = GameState.None;

        MovementComponent.Orientation = cameraOrientation;
        CameraComponent.Orientation = cameraOrientation;

        MovementState = new(this);
        DeskState = new(this);
    }

    private void Start()
    {
        ChangeState(MovementState);
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    public void ChangeState(PlayerStateBase _newState)
    {
        currentState?.ExitState();
        currentState = _newState;
        currentStateEnum = currentState.EnterState();
        if(Interactor) Interactor.UpdateState(currentStateEnum);
    }

    private void HandleMove(Vector2 _direction) => MoveDirection = _direction;
    private void HandleLook(Vector2 _direction) => LookDirection = _direction;
    private void HandlePosition(Vector2 _position) => MousePosition = _position;
}