using UnityEngine;

public interface IState
{
    public void EnterState();
    public void ExitState();
    public void UpdateState();
}

public abstract class BaseState<T> : IState
{
    protected T ctx;
    protected StateMachine<T> machine;

    protected BaseState(T _ctx, StateMachine<T> _machine)
    {
        ctx = _ctx;
        machine = _machine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void UpdateState() { }
}