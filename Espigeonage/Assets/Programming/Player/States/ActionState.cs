using UnityEngine;

public enum ActionState
{
    None = 0,
    NotGrabbing = 1,
    Grabbing = 2,
    NotDragging = 3,
    Dragging = 4
}

class NotGrabbingState : BaseState<PlayerData>
{
    public NotGrabbingState(PlayerData _ctx, StateMachine<PlayerData> _machine) : base(_ctx, _machine) { }

    public override void EnterState()
    {
        ctx.Interactor.SetActionState(ActionState.NotGrabbing);
    }

    public override void ExitState()
    {
        ctx.Interactor.SetActionState(ActionState.None);
    }

    public override void UpdateState()
    {
        ctx.Interactor.UpdateRay(ctx.CameraOrientation.position, ctx.CameraOrientation.forward);
    }
}

class GrabbingState : BaseState<PlayerData>
{
    public GrabbingState(PlayerData _ctx, StateMachine<PlayerData> _machine) : base(_ctx, _machine) { }

    public override void EnterState()
    {
        ctx.Grabber.Grab();

        ctx.Interactor.SetActionState(ActionState.Grabbing);
    }

    public override void ExitState()
    {
        ctx.Interactor.SetActionState(ActionState.None);
    }

    public override void UpdateState()
    {
        ctx.Interactor.UpdateRay(ctx.CameraOrientation.position, ctx.CameraOrientation.forward);
    }
}

class NotDraggingState : BaseState<PlayerData>
{
    public NotDraggingState(PlayerData _ctx, StateMachine<PlayerData> _machine) : base(_ctx, _machine) { }

    public override void EnterState()
    {
        ctx.Input.ExitEvent += ctx.ExitDesk;

        ctx.Interactor.SetActionState(ActionState.NotDragging);
    }

    public override void ExitState()
    {
        ctx.Input.ExitEvent -= ctx.ExitDesk;

        ctx.Interactor.SetActionState(ActionState.None);
    }

    public override void UpdateState()
    {
        ctx.Interactor.UpdateRay(Camera.main.ScreenPointToRay(ctx.MousePosition));
    }
}

class DraggingState : BaseState<PlayerData>
{
    public DraggingState(PlayerData _ctx, StateMachine<PlayerData> _machine) : base(_ctx, _machine) { }

    public override void EnterState()
    {
        ctx.Interactor.SetActionState(ActionState.Dragging);
    }

    public override void ExitState()
    {
        ctx.Interactor.SetActionState(ActionState.None);
    }

    public override void UpdateState()
    {
        ctx.Interactor.UpdateRay(Camera.main.ScreenPointToRay(ctx.MousePosition));
    }
}