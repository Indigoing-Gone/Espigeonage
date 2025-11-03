using UnityEngine;

public class LocationGrabber : Grabber, IInteractable
{
    [SerializeField] private Collider col;

    private void Awake()
    {
        GrabbableObject _grabbableChild = GetComponentInChildren<GrabbableObject>(false);
        if (_grabbableChild) GrabObject(_grabbableChild);
    }

    public void Interact(Interactor _interactor)
    {
        if (CurrentGrabbedObject) return;

        _interactor.gameObject.TryGetComponent<Grabber>(out Grabber _grabber);
        if (!_grabber) return;

        GrabObject(_grabber.CurrentGrabbedObject);
    }

    public override void GrabObject(GrabbableObject _grabbedObject)
    {
        base.GrabObject(_grabbedObject);
        col.enabled = false;
    }

    protected override void ReleaseObject()
    {
        base.ReleaseObject();
        col.enabled = true;
    }
}
