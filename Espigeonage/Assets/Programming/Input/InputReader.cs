using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputActions;

[CreateAssetMenu(menuName = "Input/InputReader")]
[DefaultExecutionOrder(-1)]
public class InputReader : ScriptableObject, IMovementActions, IInspectActions
{
    private PlayerInputActions playerInput;

    #region Callbacks

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInputActions();
            playerInput.Movement.SetCallbacks(this);
            playerInput.Inspect.SetCallbacks(this);
            DisableAll();
        }
    }

    public void SetMovement()
    {
        playerInput.Movement.Enable();
        playerInput.Inspect.Enable();
    }

    public void SetInspect()
    {
        playerInput.Movement.Disable();
        playerInput.Inspect.Enable();
    }

    public void DisableAll()
    {
        playerInput.Movement.Disable();
        playerInput.Inspect.Disable();
    }

    #endregion

    #region Events

    //Movement
    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> LookEvent;
    public event Action<bool> InteractEvent;

    //Inspect
    public event Action<bool> DragEvent;
    public event Action<Vector2> PositionEvent;
    public event Action<bool> ExitEvent;

    #endregion

    #region Triggers

    //Movement
    public void OnMove(InputAction.CallbackContext context) => MoveEvent?.Invoke(context.ReadValue<Vector2>());
    public void OnLook(InputAction.CallbackContext context) => LookEvent?.Invoke(context.ReadValue<Vector2>());
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) InteractEvent?.Invoke(true);
        if (context.phase == InputActionPhase.Canceled) InteractEvent?.Invoke(false);
    }

    //Inspect
    public void OnDrag(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) DragEvent?.Invoke(true);
        if (context.phase == InputActionPhase.Canceled) DragEvent?.Invoke(false);
    }
    public void OnPosition(InputAction.CallbackContext context) => PositionEvent?.Invoke(context.ReadValue<Vector2>());
    public void OnExit(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) ExitEvent?.Invoke(true);
        if (context.phase == InputActionPhase.Canceled) ExitEvent?.Invoke(false);
    }

    #endregion
}
