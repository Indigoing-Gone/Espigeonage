using UnityEngine;
using UnityEngine.InputSystem;

public class MovementPlayerState : PlayerStateBase
{
    public MovementPlayerState(PlayerStateMachine _player) : base(_player) { }

    public override GameState EnterState()
    {
        player.Input.SetMovement();
        player.CameraSwitcher.ChangeCamera(player.PlayerCamera);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        return GameState.Movement;
    }

    public override void UpdateState()
    {
        player.MovementComponent.SetMoveDirection(player.MoveDirection);
        player.CameraComponent.SetLookDirection(player.LookDirection);
        player.Interactor.UpdateRay(player.CameraOrientation.position, player.CameraOrientation.forward);
    }

    public override void ExitState()
    {
        player.MovementComponent.SetMoveDirection(Vector2.zero);
        player.Input.DisableAll();
    }
}
