using System;
using System.Collections.Generic;

public class StateMachine<T>
{
    private StateNode currentStateNode;
    private Dictionary<Type, StateNode> nodes = new();
    private HashSet<ITransition> anyNodeTransitions = new();

    public void Update()
    {
        ITransition _transition = GetTransition();
        if (_transition != null) ChangeState(_transition.TargetState);

        currentStateNode.State.UpdateState();
    }

    public void SetState(IState _state)
    {
        currentStateNode = nodes[_state.GetType()];
        currentStateNode.State?.EnterState();
    }

    public bool CheckState(Type _type)
    {
        return currentStateNode.State.GetType() == _type;
    }

    private void ChangeState(IState _newState)
    {
        if (_newState == currentStateNode.State) return;

        currentStateNode.State?.ExitState();
        currentStateNode = nodes[_newState.GetType()];
        currentStateNode.State?.EnterState();
    }

    private ITransition GetTransition()
    {
        foreach (ITransition _transition in anyNodeTransitions)
            if(_transition.Condition.Evaluate())
                return _transition;

        foreach (ITransition _transition in currentStateNode.Transitions)
            if (_transition.Condition.Evaluate())
                return _transition;

        return null;
    }

    public void AddTransition(IState _originState, IState _targetState, ICondition _condition) =>
        FindStateNode(_originState).AddTransition(FindStateNode(_targetState).State, _condition);

    public void AddAnyNodeTransition(IState _targetState, ICondition _condition) => 
        anyNodeTransitions.Add(new Transition(FindStateNode(_targetState).State, _condition));

    private StateNode FindStateNode(IState _state)
    {
        StateNode _node = nodes.GetValueOrDefault(_state.GetType());

        if (_node == null)
        {
            _node = new(_state);
            nodes.Add(_state.GetType(), _node);
        }

        return _node;
    }

    public class StateNode
    {
        public IState State { get; }
        public HashSet<ITransition> Transitions { get; }

        public StateNode(IState _state)
        {
            State = _state;
            Transitions = new();
        }

        public void AddTransition(IState _targetState, ICondition _condition)
        {
            Transitions.Add(new Transition(_targetState, _condition));
        }
    }
}