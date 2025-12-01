using System;
using UnityEngine;

public class Grabbable : MonoBehaviour, IGrabbable
{
    private Rigidbody rb;
    private Collider col;

    [SerializeField] private bool isDynamic = true;
    [SerializeField] private GrabbableType type;
    public GrabbableType Type => type;

    public event Action<bool> GrabbedStatus;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        rb.isKinematic = !isDynamic;
    }

    public void Grab(Grabber _grabber, Transform _grabLocation, bool _disableCollider)
    {
        if (_grabber == null) return;

        GrabbedStatus?.Invoke(true);

        rb.isKinematic = true;
        transform.parent = _grabLocation;
        SetTransform(Vector3.zero, Quaternion.identity);
        
        if(col) col.enabled = !_disableCollider;
        foreach(Collider c in GetComponentsInChildren<Collider>()) c.enabled = !_disableCollider;
    }

    public void Release()
    {
        foreach (Collider c in GetComponentsInChildren<Collider>()) c.enabled = true;
        if (col) col.enabled = true;
        transform.parent = null;
        rb.isKinematic = !isDynamic;

        GrabbedStatus?.Invoke(false);
    }

    public void SetTransform(Vector3 _position, Quaternion _rotation) => transform.SetLocalPositionAndRotation(_position, _rotation);

    public void AlignInParent(GrabbableAlignmentType _alignment)
    {
        if (transform.parent == null || 
            !BoundsUtils.TryGetLocalBoundsSelf(transform.parent, out Bounds _parentBounds) || 
            !BoundsUtils.TryGetLocalBoundsChildren(transform, out Bounds _selfBounds)) return;

        Vector3 _parentAlignPoint = Vector3.zero, _selfAlignPoint = Vector3.zero;

        switch (_alignment)
        {
            case GrabbableAlignmentType.Center:
                _parentAlignPoint = BoundsUtils.BoundsCenter(_parentBounds);
                _selfAlignPoint = BoundsUtils.BoundsCenter(_selfBounds);
                break;

            case GrabbableAlignmentType.StandingCenter:
                _parentAlignPoint = BoundsUtils.BoundsBottomCenterZ(_parentBounds);
                _selfAlignPoint = BoundsUtils.BoundsBottomCenterZ(_selfBounds);
                break;

            case GrabbableAlignmentType.LayingCenter:
                _parentAlignPoint = BoundsUtils.BoundsBottomCenterY(_parentBounds);
                _selfAlignPoint = BoundsUtils.BoundsBottomCenterY(_selfBounds);
                break;
        }

        transform.position +=
            transform.parent.TransformPoint(_parentAlignPoint) - 
            transform.TransformPoint(_selfAlignPoint);
    }
}
