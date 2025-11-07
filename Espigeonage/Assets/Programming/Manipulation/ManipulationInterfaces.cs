using System;
using UnityEngine;

public interface IInteractable
{
    public void Interact(Interactor _interactor, ActionState _currentActionState);
}

public interface IGrabbable
{
    public event Action<bool> GrabbedStatus;
    public void Grab(Grabber _grabber, Transform _grabLocation, bool _disableCollder);
    public void Release();
    public void SetTransform(Vector3 _position, Quaternion _rotation);
}

public interface IDraggable
{
    public void Drag(Dragger _dragger, Rigidbody _dragPointRb);
    public void Release();
}