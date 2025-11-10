using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputActions;

[CreateAssetMenu(menuName = "Input/InputReader")]
[DefaultExecutionOrder(-1)]
public class InputReader : ScriptableObject, IMovementActions, IInspectActions, IInteractActions
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
            playerInput.Interact.SetCallbacks(this);
            DisableAll();
        }
    }

    public void SetMovement()
    {
        playerInput.Movement.Enable();
        playerInput.Inspect.Disable();
        playerInput.Interact.Enable();
    }

    public void SetInspect()
    {
        playerInput.Movement.Disable();
        playerInput.Inspect.Enable();
        playerInput.Interact.Enable();
    }

    public void DisableAll()
    {
        playerInput.Movement.Disable();
        playerInput.Inspect.Disable();
        playerInput.Interact.Disable();
    }

    #endregion

    #region Events

    //Movement
    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> LookEvent;

    //Inspect
    public event Action<Vector2> PositionEvent;
    public event Action ExitEvent;
    public event Action<Vector2> PathEvent;

    //Interact
    public event Action<bool> InteractEvent;

    #endregion

    #region Triggers

    //Movement
    public void OnMove(InputAction.CallbackContext context) => MoveEvent?.Invoke(context.ReadValue<Vector2>());
    public void OnLook(InputAction.CallbackContext context) => LookEvent?.Invoke(context.ReadValue<Vector2>());

    //Inspect
    public void OnPosition(InputAction.CallbackContext context) => PositionEvent?.Invoke(context.ReadValue<Vector2>());
    public void OnExit(InputAction.CallbackContext context) { if (context.phase == InputActionPhase.Performed) ExitEvent?.Invoke(); }
    public void OnPath(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && context.ReadValue<Vector2>() != Vector2.zero)
            PathEvent?.Invoke(context.ReadValue<Vector2>());
    }


    //Interact
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) InteractEvent?.Invoke(true);
        if (context.phase == InputActionPhase.Canceled) InteractEvent?.Invoke(false);
    }
    #endregion
}
