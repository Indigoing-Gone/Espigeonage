using System;
using Unity.VisualScripting;
using UnityEngine;

public class DeskPlayerState : PlayerStateBase
{
    private Desk currentDesk;
    private bool deskInUse;

    public DeskPlayerState(PlayerStateMachine _player) : base(_player) { }

    public override GameState EnterState()
    {
        player.Input.SetInspect();
        player.CameraSwitcher.ChangeCamera(currentDesk.DeskCamera);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        deskInUse = true;

        player.Input.InteractEvent += HandleDragRelease;
        player.Input.ExitEvent += OnExitDesk;

        return GameState.Desk;
    }


    public override void UpdateState()
    {
        player.Dragger.AttemptDrag(player.MousePosition);
        player.Interactor.UpdateRay(Camera.main.ScreenPointToRay(player.MousePosition));
    }

    public override void ExitState()
    {
        player.Input.DisableAll();

        player.Input.InteractEvent -= HandleDragRelease;
        player.Input.ExitEvent -= OnExitDesk;
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
            player.ChangeState(player.MovementState);
            deskInUse = false;
        }
    }

    private void HandleDragRelease(bool _state)
    {
        if (!_state) player.Dragger.HandleRelease();
    }
}
