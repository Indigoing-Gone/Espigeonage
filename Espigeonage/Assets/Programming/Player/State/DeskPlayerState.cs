using UnityEngine;

public class DeskPlayerState : PlayerStateBase
{
    private Desk currentDesk;
    private bool deskInUse;

    public DeskPlayerState(PlayerStateManager _manager) : base(_manager) { }

    public override GameState EnterState()
    {
        manager.Input.SetInspect();
        manager.CameraSwitcher.ChangeCamera(currentDesk.DeskCamera);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        deskInUse = true;

        manager.Input.ExitEvent += OnExitDesk;

        return GameState.Desk;
    }

    public override void ExitState()
    {
        manager.Input.DisableAll();
        manager.Input.ExitEvent -= OnExitDesk;
    }

    public DeskPlayerState UpdateDesk(Desk _newDesk)
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
