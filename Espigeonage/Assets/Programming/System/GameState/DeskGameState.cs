using UnityEngine;

public class DeskGameState : GameStateBase
{
    private Desk currentDesk;
    private bool deskInUse;

    public DeskGameState(GameStateManager _manager, GameContext _context) : base(_manager, _context) { }

    public override void EnterState()
    {
        context.Input.SetInspect();
        context.CameraManager.ChangeCamera(currentDesk.DeskCamera);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        deskInUse = true;

        context.Input.ExitEvent += OnExitDesk;
    }

    public override void ExitState()
    {
        context.Input.DisableAll();
        context.Input.ExitEvent -= OnExitDesk;
    }

    public DeskGameState UpdateDesk(Desk _newDesk)
    {
        currentDesk = _newDesk;
        return this;
    }

    private void OnExitDesk(bool _state)
    {
        if (deskInUse && _state)
        {
            manager.ChangeState(manager.MovementState);
            deskInUse = false;
        }
    }
}
