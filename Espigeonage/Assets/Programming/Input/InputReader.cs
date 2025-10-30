using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputActions;

[CreateAssetMenu(menuName = "Input/InputReader")]
[DefaultExecutionOrder(-1)]
public class InputReader : ScriptableObject, IMovementActions
{
    private PlayerInputActions playerInput;

    #region Callbacks

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInputActions();
            playerInput.Movement.SetCallbacks(this);
            DisableAll();
        }
    }

    public void SetMovement()
    {
        playerInput.Movement.Enable();
    }

    public void DisableAll()
    {
        playerInput.Movement.Disable();
    }

    #endregion

    #region Events

    //Movement
    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> LookEvent;
    public event Action<bool> InteractEvent;

    #endregion

    #region Triggers

    public void OnMove(InputAction.CallbackContext context) => MoveEvent?.Invoke(context.ReadValue<Vector2>());

    public void OnLook(InputAction.CallbackContext context) => LookEvent?.Invoke(context.ReadValue<Vector2>());

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) InteractEvent?.Invoke(true);
        if (context.phase == InputActionPhase.Canceled) InteractEvent?.Invoke(false);
    }

    #endregion
}
