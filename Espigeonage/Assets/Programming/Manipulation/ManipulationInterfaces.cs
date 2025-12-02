using System;
using UnityEngine;

public enum GrabbableAlignmentType
{
    Center,
    StandingCenter,
    LayingCenter
}

public interface IInteractable
{
    public void Interact(Interactor _interactor, ActionState _currentActionState);
    public bool TryFindInteraction(ActionState _requiredState, out InteractionData _inputInteraction);
}

public interface IGrabbable
{
    public GrabbableType Type { get; }

    public event Action<bool> GrabbedStatus;

    public void Grab(Grabber _grabber, GrabData _grabData);
    public void Release();

    public void SetTransform(Vector3 _position, Quaternion _rotation);
    public void AlignInParent(GrabbableAlignmentType _alignment);
}

public interface IDraggable
{
    public void Drag(Dragger _dragger, Rigidbody _dragPointRb);
    public void Release();
}