using System;

public interface ICondition
{
    public bool Evaluate();
}

public class ActionCondition : ICondition
{
    private event Action Action;
    private bool eventRaised;

    public ActionCondition(Action _action)
    {
        Action = _action;
        eventRaised = false;
        Action += () => eventRaised = true;
    }

    public bool Evaluate()
    {
        if (!eventRaised) return false;
        eventRaised = false;
        return true;
    }
}

public class FuncCondition : ICondition
{
    private Func<bool> func;

    public FuncCondition(Func<bool> _func) => func = _func;
    public bool Evaluate() => func.Invoke();
}