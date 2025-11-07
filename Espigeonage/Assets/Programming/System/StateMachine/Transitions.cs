public interface ITransition
{
    public IState TargetState { get; }
    public ICondition Condition { get; }
}

public class Transition : ITransition
{
    public IState TargetState { get; }
    public ICondition Condition { get; }

    public Transition(IState _targetState, ICondition _condition)
    {
        TargetState = _targetState;
        Condition = _condition;
    }
}