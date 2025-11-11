using UnityEngine;

public class SlotGrabber : Grabber
{
    protected Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();

        IGrabbable _grabbableChild = GetComponentInChildren<IGrabbable>(false);
        if (_grabbableChild != null)
        {
            SetGrabbable(_grabbableChild);
            Grab();
        }
    }

    public override void Grab()
    {
        base.Grab();
        currentGrabbable.GrabbedStatus += OnGrabbableGrabbed;
        col.enabled = false;
    }

    public override IGrabbable Release()
    {
        col.enabled = true;
        currentGrabbable.GrabbedStatus -= OnGrabbableGrabbed;
        return base.Release();
    }

    private void OnGrabbableGrabbed(bool _state) { if (_state) Release(); }
}
