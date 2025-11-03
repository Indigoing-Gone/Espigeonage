using UnityEngine;

public class PlayerGrabber : Grabber
{
    public override void GrabObject(GrabbableObject _grabbedObject)
    {
        base.GrabObject(_grabbedObject);

        CurrentGrabbedObject.Col.enabled = false;
    }

    protected override void ReleaseObject()
    {
        if (CurrentGrabbedObject) CurrentGrabbedObject.Col.enabled = true;
        base.ReleaseObject();
    }
}
