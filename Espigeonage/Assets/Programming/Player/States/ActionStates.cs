using System.Collections;
using UnityEngine;

public enum ActionState
{
    None = 0,
    NotGrabbing = 1,
    Grabbing = 2,
    NotDragging = 3,
    Dragging = 4,
    Inspecting = 5,
    Drawing = 6,
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
        ctx.Interactor.FindInteractables();
    }
}

class GrabbingState : BaseState<PlayerData>
{
    public GrabbingState(PlayerData _ctx, StateMachine<PlayerData> _machine) : base(_ctx, _machine) { }

    public override void EnterState()
    {
        ctx.Grabber.Grab();
        ctx.Input.ExitEvent += ctx.Grabber.ToggleInspecting;
        ctx.PlayerUI.SetSecondaryTooltip("[RMB] Inspect", true);

        ctx.Interactor.SetActionState(ActionState.Grabbing);
    }

    public override void ExitState()
    {
        ctx.Input.ExitEvent -= ctx.Grabber.ToggleInspecting;
        ctx.PlayerUI.SetSecondaryTooltip("NULL", false);

        ctx.Interactor.SetActionState(ActionState.None);
    }

    public override void UpdateState()
    {
        ctx.Interactor.UpdateRay(ctx.CameraOrientation.position, ctx.CameraOrientation.forward);
        ctx.Interactor.FindInteractables();
    }
}

class NotDraggingState : BaseState<PlayerData>
{
    public NotDraggingState(PlayerData _ctx, StateMachine<PlayerData> _machine) : base(_ctx, _machine) { }

    public override void EnterState()
    {
        ctx.Input.ExitEvent += ctx.ExitDesk;
        ctx.PlayerUI.SetSecondaryTooltip("[RMB] Exit Desk", true);

        ctx.Interactor.SetActionState(ActionState.NotDragging);
    }

    public override void ExitState()
    {
        ctx.Input.ExitEvent -= ctx.ExitDesk;
        ctx.PlayerUI.SetSecondaryTooltip("NULL", false);

        ctx.Interactor.SetActionState(ActionState.None);
    }

    public override void UpdateState()
    {
        ctx.Dragger.UpdateDragPosition(ctx.MousePosition);

        ctx.Interactor.UpdateRay(ctx.MousePosition);
        ctx.Interactor.FindInteractables();
    }
}

class DraggingState : BaseState<PlayerData>
{
    public DraggingState(PlayerData _ctx, StateMachine<PlayerData> _machine) : base(_ctx, _machine) { }

    public override void EnterState()
    {
        ctx.Dragger.UpdateDragPosition(ctx.MousePosition);
        ctx.Dragger.Drag();

        ctx.Input.InteractEvent += ctx.ReleaseDrag;

        ctx.Interactor.SetActionState(ActionState.Dragging);
    }

    public override void ExitState()
    {
        ctx.Input.InteractEvent -= ctx.ReleaseDrag;

        ctx.Interactor.SetActionState(ActionState.None);
    }

    public override void UpdateState()
    {
        ctx.Dragger.UpdateDragPosition(ctx.MousePosition);

        ctx.Interactor.UpdateRay(ctx.MousePosition);
        ctx.Interactor.FindInteractables();
    }
}

class InspectingState : BaseState<PlayerData>
{
    public InspectingState(PlayerData _ctx, StateMachine<PlayerData> _machine) : base(_ctx, _machine) { }

    public override void EnterState()
    {
        ctx.Grabber.Inspect();

        ctx.Input.ExitEvent += ctx.Grabber.ToggleInspecting;
        ctx.PlayerUI.SetSecondaryTooltip("[RMB] Stop Inspecting", true);

        ctx.Interactor.SetActionState(ActionState.Inspecting);
    }

    public override void ExitState()
    {
        ctx.Input.ExitEvent -= ctx.Grabber.ToggleInspecting;
        ctx.PlayerUI.SetSecondaryTooltip("NULL", false);

        ctx.Interactor.SetActionState(ActionState.None);
    }

    public override void UpdateState()
    {
        ctx.Dragger.UpdateDragPosition(ctx.MousePosition);

        ctx.Interactor.UpdateRay(ctx.MousePosition);
        ctx.Interactor.FindInteractables();
    }
}

class DrawingState : BaseState<PlayerData>
{
    public DrawingState(PlayerData _ctx, StateMachine<PlayerData> _machine) : base(_ctx, _machine) { }

    public override void EnterState()
    {
        ctx.Drawer.UpdateDrawPosition(ctx.MousePosition);
        ctx.Drawer.StartDrawing();

        ctx.Input.InteractEvent += ctx.StopDrawing;

        ctx.Interactor.SetActionState(ActionState.Drawing);
    }

    public override void ExitState()
    {
        ctx.Input.InteractEvent -= ctx.StopDrawing;

        ctx.Interactor.SetActionState(ActionState.None);
    }

    public override void UpdateState()
    {
        ctx.Drawer.UpdateDrawPosition(ctx.MousePosition);
    }
}