public abstract class PlayerStateBase
{
    protected PlayerStateMachine player;

    protected PlayerStateBase(PlayerStateMachine _player)
    {
        player = _player;
    }

    public abstract GameState EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
