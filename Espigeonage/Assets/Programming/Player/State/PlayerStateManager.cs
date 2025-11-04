using System;
using UnityEngine;

public enum GameState
{
    None = 0,
    Movement = 1,
    Desk = 2,
}

public class PlayerStateManager : MonoBehaviour
{
    [Header("Components")]
    public Player Player { get; private set; }
    public CameraSwitcher CameraSwitcher { get; private set; }
    private Interactor interactor;

    [SerializeField] private InputReader input;
    public InputReader Input => input;

    [Header("States")]
    public GameState currentStateEnum;
    private PlayerStateBase currentState;
    public MovementPlayerState MovementState { get; private set; }
    public DeskPlayerState DeskState { get; private set; }

    private void Awake()
    {
        Player = GetComponent<Player>();
        CameraSwitcher = GetComponent<CameraSwitcher>();
        interactor = GetComponent<Interactor>();

        currentStateEnum = GameState.None;

        MovementState = new(this, Player);
        DeskState = new(this);
    }

    private void Start()
    {
        ChangeState(MovementState);
    }

    public void ChangeState(PlayerStateBase _newState)
    {
        currentState?.ExitState();
        currentState = _newState;
        currentStateEnum = currentState.EnterState();
        if(interactor) interactor.UpdateState(currentStateEnum);
    }
}