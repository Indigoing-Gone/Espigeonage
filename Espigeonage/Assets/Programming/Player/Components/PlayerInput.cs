using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Input/PlayerInput")]
public class PlayerInput : ScriptableObject
{
    [SerializeField] private InputReader input;

    private Vector2 moveDirection;
    private Vector2 lookDirection;

    public Vector2 MoveDirection { get => moveDirection; }
    public Vector2 LookDirection { get => lookDirection; }
    public event Action<bool> InteractStatus;

    private void OnEnable()
    {
        input.MoveEvent += HandleMove;
        input.LookEvent += HandleLook;
        input.InteractEvent += HandleInteract;
    }

    private void OnDisable()
    {
        input.MoveEvent -= HandleMove;
        input.LookEvent -= HandleLook;
        input.InteractEvent -= HandleInteract;
    }

    private void HandleMove(Vector2 _direction) => moveDirection = _direction;
    private void HandleLook(Vector2 _direction) => lookDirection = _direction;
    private void HandleInteract(bool _state) => InteractStatus?.Invoke(_state);
}
