using UnityEngine;

public class SlotGrabber : Grabber
{
    [SerializeField] private Collider col;

    private void Awake()
    {
        IGrabbable _grabbableChild = GetComponentInChildren<IGrabbable>(false);
        if (_grabbableChild != null) HandleGrab(_grabbableChild);
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
