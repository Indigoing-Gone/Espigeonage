using UnityEngine;

public class LocationGrabber : Grabber, IInteractable
{
    [SerializeField] private Collider col;

    private void Awake()
    {
        IGrabbable _grabbableChild = GetComponentInChildren<IGrabbable>(false);
        if (_grabbableChild != null) HandleGrab(_grabbableChild);
    }

    public void Interact(Interactor _interactor)
    {
        if (currentGrabbable != null) return;

        _interactor.gameObject.TryGetComponent<Grabber>(out Grabber _grabber);
        if (!_grabber) return;

        HandleGrab(_grabber.CurrentGrabbable);
    }

    public override void HandleGrab(IGrabbable _grabbable)
    {
        col.enabled = false;
        base.HandleGrab(_grabbable);
    }

    public override void HandleRelease()
    {
        col.enabled = true;
        base.HandleRelease();
    }
}
