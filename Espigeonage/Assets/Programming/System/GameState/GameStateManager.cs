using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private GameStateBase currentState;
    private GameContext context;

    public MovementGameState MovementState { get; private set; }
    public DeskGameState DeskState { get; private set; }

    private void Awake()
    {
        context = GetComponent<GameContext>();

        MovementState = new(this, context, context.Player);
        DeskState = new(this, context);
        ChangeState(MovementState);
    }

    public void ChangeState(GameStateBase _newState)
    {
        currentState?.ExitState();
        currentState = _newState;
        currentState.EnterState();
    }
}