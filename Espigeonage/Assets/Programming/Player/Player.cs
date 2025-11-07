using UnityEngine;

[RequireComponent(typeof(PlayerData))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    private PlayerData data;

    [Header("State Machine")]
    private StateMachine<PlayerData> MotionStateMachine;
    private StateMachine<PlayerData> ActionStateMachine;

    private void Awake()
    {
        data = GetComponent<PlayerData>();
    }

    private void Start()
    {
        InitializeStateMachines();
    }

    private void InitializeStateMachines()
    {
        //State Machines
        MotionStateMachine = new();
        ActionStateMachine = new();

        //States
        BaseState<PlayerData> movementState = new MovementState(data, MotionStateMachine);
        BaseState<PlayerData> deskState = new DeskState(data, MotionStateMachine);

        BaseState<PlayerData> notGrabbingState = new NotGrabbingState(data, ActionStateMachine);
        BaseState<PlayerData> grabbingState = new GrabbingState(data, ActionStateMachine);
        BaseState<PlayerData> notDraggingState = new NotDraggingState(data, ActionStateMachine);
        BaseState<PlayerData> draggingState = new DraggingState(data, ActionStateMachine);

        //Locomotion Transitions
        MotionStateMachine.AddTransition(movementState, deskState, new FuncCondition(() => data.AtDesk));
        MotionStateMachine.AddTransition(deskState, movementState, new FuncCondition(() => !data.AtDesk));

        //Action Transitions
        ActionStateMachine.AddTransition(notGrabbingState, notDraggingState, new FuncCondition(() => MotionStateMachine.CheckState(typeof(DeskState))));
        ActionStateMachine.AddTransition(notDraggingState, notGrabbingState, new FuncCondition(() => MotionStateMachine.CheckState(typeof(MovementState))));

        ActionStateMachine.AddTransition(notGrabbingState, grabbingState, new FuncCondition(() => data.Grabber.HasGrabbable));
        ActionStateMachine.AddTransition(grabbingState, notGrabbingState, new FuncCondition(() => !data.Grabber.HasGrabbable));

        ActionStateMachine.AddTransition(notDraggingState, draggingState, new FuncCondition(() => data.Dragger.HasDraggable));
        ActionStateMachine.AddTransition(draggingState, notDraggingState, new FuncCondition(() => !data.Dragger.HasDraggable));

        //Set Initial State
        MotionStateMachine.SetState(movementState);
        ActionStateMachine.SetState(notGrabbingState);
    }

    private void Update()
    {
        MotionStateMachine.Update();
        ActionStateMachine.Update();
    }
}
