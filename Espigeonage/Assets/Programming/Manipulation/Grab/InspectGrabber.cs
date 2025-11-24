using UnityEngine;

public class InspectGrabber : Grabber
{
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

    public void ToggleInspecting() { IsInspecting = !IsInspecting; Debug.Log(IsInspecting); }
}
