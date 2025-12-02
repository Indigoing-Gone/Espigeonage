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
        if (transform.parent == null) return;
        Transform _boundsTransform = transform.parent.GetComponentInChildren<AlignmentBounds>().transform;
        if(_boundsTransform == null) return;

        if (!BoundsUtils.TryGetLocalBoundsChildren(_boundsTransform, out Bounds _alignBounds) || 
            !BoundsUtils.TryGetLocalBoundsChildren(transform, out Bounds _selfBounds)) return;

        Vector3 _boundingAlignPoint = Vector3.zero, _selfAlignPoint = Vector3.zero;

        switch (_alignment)
        {
            case GrabbableAlignmentType.Center:
                _boundingAlignPoint = BoundsUtils.BoundsCenter(_alignBounds);
                _selfAlignPoint = BoundsUtils.BoundsCenter(_selfBounds);
                break;

            case GrabbableAlignmentType.StandingCenter:
                _boundingAlignPoint = BoundsUtils.BoundsBottomCenterZ(_alignBounds);
                _selfAlignPoint = BoundsUtils.BoundsBottomCenterZ(_selfBounds);
                break;

            case GrabbableAlignmentType.LayingCenter:
                _boundingAlignPoint = BoundsUtils.BoundsBottomCenterY(_alignBounds);
                _selfAlignPoint = BoundsUtils.BoundsBottomCenterY(_selfBounds);
                break;
        }

        transform.position +=
            _boundsTransform.TransformPoint(_boundingAlignPoint) - 
            transform.TransformPoint(_selfAlignPoint);
    }
}
