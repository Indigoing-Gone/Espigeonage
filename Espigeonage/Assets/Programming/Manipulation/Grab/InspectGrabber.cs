using UnityEngine;

public class InspectGrabber : Grabber
{
    [Header("Inspect Parameters")]
    [SerializeField] protected GrabData inspectData;
    public bool IsInspecting { get; private set; }

    public override void Grab()
    {
        IsInspecting = false;
        base.Grab();
    }

    public override IGrabbable Release()
    {
        IsInspecting = false;
        return base.Release();
    }

    public virtual void Inspect()
    {
        currentGrabbable?.Grab(this, inspectData);

        MonoBehaviour _grabbableObject = currentGrabbable as MonoBehaviour;
        if (_grabbableObject == null) return;

        _grabbableObject.TryGetComponent<Blueprint>(out Blueprint _blueprint);
        if (_blueprint == null) return;
        _blueprint.UnlockModification();
    }

    public void ToggleInspecting() { IsInspecting = !IsInspecting; }
}
