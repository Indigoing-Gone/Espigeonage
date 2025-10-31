using UnityEngine;

public class GrabbableObject : MonoBehaviour, IInteractable
{
    private bool isGrabbed;

    public void Interact(Interactor _interactor)
    {
        if (isGrabbed) return;

        _interactor.gameObject.TryGetComponent<Grabber>(out Grabber _grabber);
        if (!_grabber) return;
        _grabber.GrabObject(this);
    }
}
