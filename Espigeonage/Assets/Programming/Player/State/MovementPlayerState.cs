using UnityEngine;

public class MovementPlayerState : PlayerStateBase
{
    private Player player;

    public MovementPlayerState(PlayerStateManager _manager, Player _player) : base(_manager)
    {
        player = _player;
    }

    public override GameState EnterState()
    {
        manager.Input.SetMovement();
        manager.CameraSwitcher.ChangeCamera(player.Cam);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        return GameState.Movement;
    }

    public override void ExitState()
    {
        manager.Input.DisableAll();
    }
}
