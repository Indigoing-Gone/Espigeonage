using UnityEngine;

public class Grabber : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform grabLocation;

    public GrabbableObject CurrentGrabbedObject { get; protected set; }

    public virtual void GrabObject(GrabbableObject _grabbedObject)
    {
        if (CurrentGrabbedObject || CurrentGrabbedObject == _grabbedObject) return;

        CurrentGrabbedObject = _grabbedObject;
        CurrentGrabbedObject.OnObjectGrabbed();

        CurrentGrabbedObject.transform.parent = grabLocation;
        CurrentGrabbedObject.transform.localPosition = Vector3.zero;

        CurrentGrabbedObject.ObjectGrabbed += ReleaseObject;
    }

    protected virtual void ReleaseObject()
    {
        if (!CurrentGrabbedObject) return;

        CurrentGrabbedObject.ObjectGrabbed -= ReleaseObject;

        CurrentGrabbedObject.transform.parent = null;
        CurrentGrabbedObject = null;
    }
}
