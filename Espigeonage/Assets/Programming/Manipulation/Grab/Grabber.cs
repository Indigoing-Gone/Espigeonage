using System;
using UnityEngine;

[Flags]
public enum GrabbableType
{
    None,
    Default,
    Book,
    Blueprint 
}

public class Grabber : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected Transform grabLocation;
    protected IGrabbable currentGrabbable;
    public IGrabbable CurrentGrabbable => currentGrabbable;

    [Header("Grab Parameters")]
    [SerializeField] protected bool disableGrabbableCollider;
    [SerializeField] protected GrabbableType vaildGrabbables;
    public bool HasGrabbable => currentGrabbable != null;

    public void SetGrabbable(IGrabbable _grabbable)
    {
        if (currentGrabbable != null) return;
        currentGrabbable = _grabbable;
    }

    public virtual void Grab()
    {
        currentGrabbable?.Grab(this, grabLocation, disableGrabbableCollider);
    }

    public virtual IGrabbable Release()
    {
        IGrabbable _releasedGrabbable = currentGrabbable;
        currentGrabbable?.Release();
        currentGrabbable = null;
        return _releasedGrabbable;
    }
}
