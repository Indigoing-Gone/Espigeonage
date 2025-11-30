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
        BaseState<PlayerData> frozenState = new FrozenState(data, MotionStateMachine);

        BaseState<PlayerData> notGrabbingState = new NotGrabbingState(data, ActionStateMachine);
        BaseState<PlayerData> grabbingState = new GrabbingState(data, ActionStateMachine);
        BaseState<PlayerData> notDraggingState = new NotDraggingState(data, ActionStateMachine);
        BaseState<PlayerData> draggingState = new DraggingState(data, ActionStateMachine);
        BaseState<PlayerData> inspectingState = new InspectingState(data, ActionStateMachine);
        BaseState<PlayerData> drawingState = new DrawingState(data, ActionStateMachine);

        //Locomotion Transitions

        //Movement <-> Desk
        MotionStateMachine.AddTransition(movementState, deskState, new FuncCondition(() => data.AtDesk));
        MotionStateMachine.AddTransition(deskState, movementState, new FuncCondition(() => !data.AtDesk));

        //Movement <-> Inspect
        MotionStateMachine.AddTransition(movementState, frozenState, new FuncCondition(() => data.Grabber.IsInspecting));
        MotionStateMachine.AddTransition(frozenState, movementState, new FuncCondition(() => !data.Grabber.IsInspecting));


        //Action Transitions

        //NotGrabbing <-> NotDragging
        ActionStateMachine.AddTransition(notGrabbingState, notDraggingState, new FuncCondition(() => MotionStateMachine.CheckState(typeof(DeskState))));
        ActionStateMachine.AddTransition(notDraggingState, notGrabbingState, new FuncCondition(() => MotionStateMachine.CheckState(typeof(MovementState))));

        //NotGrabbing <-> Grabbing
        ActionStateMachine.AddTransition(notGrabbingState, grabbingState, new FuncCondition(() => data.Grabber.HasGrabbable));
        ActionStateMachine.AddTransition(grabbingState, notGrabbingState, new FuncCondition(() => !data.Grabber.HasGrabbable));

        //Grabbing <-> Inspecting
        ActionStateMachine.AddTransition(grabbingState, inspectingState, new FuncCondition(() => data.Grabber.IsInspecting));
        ActionStateMachine.AddTransition(inspectingState, grabbingState, new FuncCondition(() => !data.Grabber.IsInspecting));

        //NotDragging <-> Dragging
        ActionStateMachine.AddTransition(notDraggingState, draggingState, new FuncCondition(() => data.Dragger.HasDraggable));
        ActionStateMachine.AddTransition(draggingState, notDraggingState, new FuncCondition(() => !data.Dragger.HasDraggable));

        //NotDragging <-> Drawing
        ActionStateMachine.AddTransition(notDraggingState, drawingState, new FuncCondition(() => data.Drawer.HasDrawable));
        ActionStateMachine.AddTransition(drawingState, notDraggingState, new FuncCondition(() => !data.Drawer.HasDrawable));


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
