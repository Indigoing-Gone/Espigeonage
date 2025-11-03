using UnityEngine;

public class MovementGameState : GameStateBase
{
    private Player player;

    public MovementGameState(GameStateManager _manager, GameContext _context, Player _player) : base(_manager, _context)
    {
        player = _player;
    }

    public override void EnterState()
    {
        context.Input.SetMovement();
        context.CameraManager.ChangeCamera(player.Cam);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        DeskEnterer.EnteringDesk += OnEnteringDesk;
    }

    public override void ExitState()
    {
        context.Input.DisableAll();
        DeskEnterer.EnteringDesk -= OnEnteringDesk;
    }

    private void OnEnteringDesk(Desk _desk)
    {
        manager.ChangeState(manager.DeskState.UpdateDesk(_desk));
    }
}
