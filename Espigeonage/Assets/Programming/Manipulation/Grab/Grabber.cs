using UnityEngine;

public class Grabber : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected Transform grabLocation;
    public Transform GrabLocation => grabLocation;

    [Header("Grab Logic")]
    [SerializeField] protected bool disableGrabbableCollider;
    public bool DisableGrabbableCollider => disableGrabbableCollider;
    [SerializeField] protected IGrabbable currentGrabbable;
    public IGrabbable CurrentGrabbable => currentGrabbable;

    public virtual void HandleGrab(IGrabbable _grabbable)
    {
        //We are already grabbing something, we regrabbed our current grabbable, or the new grabbable couldn't be grabbed
        if (currentGrabbable != null || currentGrabbable == _grabbable || _grabbable.Grab(this) == false) return;
        currentGrabbable = _grabbable;

        currentGrabbable.BeingGrabbed += HandleRelease;
    }

    public virtual void HandleRelease()
    {
        if (currentGrabbable == null) return;

        currentGrabbable.BeingGrabbed -= HandleRelease;

        currentGrabbable.Release();
        currentGrabbable = null;
    }
}
