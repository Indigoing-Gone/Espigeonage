public abstract class PlayerStateBase
{
    protected PlayerStateManager manager;

    protected PlayerStateBase(PlayerStateManager _manager)
    {
        manager = _manager;
    }

    public abstract GameState EnterState();
    public abstract void ExitState();
}
