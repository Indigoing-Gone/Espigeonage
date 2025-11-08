using UnityEngine;

class MovementState : BaseState<PlayerData>
{
    public MovementState(PlayerData _ctx, StateMachine<PlayerData> _machine) : base(_ctx, _machine) { }

    public override void EnterState()
    {
        ctx.Input.SetMovement();
        ctx.CameraSwitcher.ChangeCamera(ctx.PlayerCamera);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void ExitState()
    {
        ctx.MovementComponent.SetMoveDirection(Vector2.zero);
        ctx.CameraComponent.SetLookDirection(Vector2.zero);

        ctx.Input.DisableAll();
    }

    public override void UpdateState()
    {
        ctx.MovementComponent.SetMoveDirection(ctx.MoveDirection);
        ctx.CameraComponent.SetLookDirection(ctx.LookDirection);
    }
}

class DeskState : BaseState<PlayerData>
{
    public DeskState(PlayerData _ctx, StateMachine<PlayerData> _machine) : base(_ctx, _machine) { }

    public override void EnterState()
    {
        ctx.Input.SetInspect();
        ctx.CameraSwitcher.ChangeCamera(ctx.CurrentDesk != null ? ctx.CurrentDesk.DeskCamera : null);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public override void ExitState()
    {
        ctx.Input.DisableAll();
    }
}