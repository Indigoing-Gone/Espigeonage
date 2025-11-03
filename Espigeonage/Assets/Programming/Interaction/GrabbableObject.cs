using System;
using UnityEngine;

public class GrabbableObject : MonoBehaviour, IInteractable
{
    public Collider Col { get; private set; }
    public event Action ObjectGrabbed;

    private void Awake()
    {
        Col = GetComponent<Collider>();
    }

    public void Interact(Interactor _interactor)
    {
        _interactor.gameObject.TryGetComponent<Grabber>(out Grabber _grabber);
        if (!_grabber) return;

        _grabber.GrabObject(this);
    }

    public void OnObjectGrabbed() => ObjectGrabbed?.Invoke();
}
