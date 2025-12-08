using System;
using UnityEngine;

public class Grabbable : MonoBehaviour, IGrabbable
{
    [Header("Components")]
    private Rigidbody rb;
    private Collider col;

    [SerializeField] private GameObject defaultModel;
    [SerializeField] private GameObject grabbedModel;

    [Header("Grabable Parameters")]
    [SerializeField] private bool isDynamic = true;
    [SerializeField] private GrabbableType type;
    public GrabbableType Type => type;
    [SerializeField] Vector3 grabbedRotation;

    public event Action<bool> GrabbedStatus;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        rb.isKinematic = !isDynamic;
        if(grabbedModel != null) grabbedModel.SetActive(false);
    }

    public void Grab(Grabber _grabber, GrabData _grabData)
    {
        if (_grabber == null) return;

        GrabbedStatus?.Invoke(true);

        //Handle parent and position
        rb.isKinematic = true;
        transform.parent = _grabData.location;
        Quaternion _rotation = _grabData.rotateObject ? Quaternion.Euler(grabbedRotation) : Quaternion.identity;
        SetTransform(Vector3.zero, _rotation);
        
        //Handle collider modification
        if(col) col.enabled = !_grabData.disableCollider;
        foreach(Collider c in GetComponentsInChildren<Collider>()) c.enabled = !_grabData.disableCollider;

        //Handle model modification
        if(_grabData.swapModel && defaultModel != null && grabbedModel != null)
        {
            defaultModel.SetActive(false);
            grabbedModel.SetActive(true);
        }
    }

    public void Release()
    {
        //Handle model modification
        if (defaultModel != null && grabbedModel != null)
        {
            defaultModel.SetActive(true);
            grabbedModel.SetActive(false);
        }

        //Handle collider Modification
        foreach (Collider c in GetComponentsInChildren<Collider>()) c.enabled = true;
        if (col) col.enabled = true;

        //Handle parent and position
        transform.parent = null;
        rb.isKinematic = !isDynamic;

        GrabbedStatus?.Invoke(false);
    }

    public void SetTransform(Vector3 _position, Quaternion _rotation) => transform.SetLocalPositionAndRotation(_position, _rotation);

    public void AlignInParent(GrabbableAlignmentType _alignment)
    {
        if (transform.parent == null) return;
        AlignmentBounds _bounds = transform.parent.GetComponentInChildren<AlignmentBounds>();
        if (_bounds == null) return;
        Transform _boundsTransform = _bounds.transform;

        if (!BoundsUtils.TryGetLocalBoundsChildren(_boundsTransform, _boundsTransform, out Bounds _alignBounds) || 
            !BoundsUtils.TryGetLocalBoundsChildren(transform, transform, out Bounds _selfBounds)) return;

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

            case GrabbableAlignmentType.TopCorner:
                _boundingAlignPoint = BoundsUtils.BoundsTopCorner(_alignBounds);
                _selfAlignPoint = BoundsUtils.BoundsTopCorner(_selfBounds);
                break;
        }

        transform.position +=
            _boundsTransform.TransformPoint(_boundingAlignPoint) - 
            transform.TransformPoint(_selfAlignPoint);
    }
}
