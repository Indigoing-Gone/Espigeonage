using System;
using Unity.VisualScripting;
using UnityEngine;

[Flags]
public enum GrabbableType
{
    None = 0,
    Default = 1,
    Book = 2,
    Blueprint = 4,
    SmallTrinket = 8,
    BigTrinket = 16,
}

[Serializable]
public struct GrabData
{
    public Transform location;
    public bool disableCollider;
    public bool swapModel;
}

public class Grabber : MonoBehaviour
{
    [Header("Components")]
    protected IGrabbable currentGrabbable;
    public IGrabbable CurrentGrabbable => currentGrabbable;

    [Header("Grab Parameters")]
    [SerializeField] protected GrabData grabData;
    [SerializeField] protected GrabbableType vaildGrabbables;
    [SerializeField] protected GrabbableAlignmentType grabbableAlignment;
    public bool HasGrabbable => currentGrabbable != null;

    public void SetGrabbable(IGrabbable _grabbable)
    {
        if (currentGrabbable != null) return;
        currentGrabbable = _grabbable;
    }

    public bool CanGrab(IGrabbable _grabbable) => vaildGrabbables.HasFlag(_grabbable.Type);

    public virtual void Grab()
    {
        currentGrabbable?.Grab(this, grabData);
        currentGrabbable.AlignInParent(grabbableAlignment);
    }

    public virtual IGrabbable Release()
    {
        IGrabbable _releasedGrabbable = currentGrabbable;
        currentGrabbable?.Release();
        currentGrabbable = null;
        return _releasedGrabbable;
    }
}
