public abstract class GameStateBase
{
    protected GameStateManager manager;
    protected GameContext context;

    protected GameStateBase(GameStateManager _manager, GameContext _context)
    {
        manager = _manager;
        context = _context;
    }

    public abstract void EnterState();
    public abstract void ExitState();
}
