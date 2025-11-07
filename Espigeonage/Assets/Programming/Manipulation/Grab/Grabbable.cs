using System;
using UnityEngine;

public class Grabbable : MonoBehaviour, IGrabbable
{
    private Rigidbody rb;
    private Collider col;

    [SerializeField] private bool isDynamic;

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
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        col.enabled = !_disableCollider;
    }

    public void Release()
    {
        col.enabled = true;
        transform.parent = null;
        rb.isKinematic = !isDynamic;

        GrabbedStatus?.Invoke(false);
    }

    public void SetTransform(Vector3 _position, Quaternion _rotation) => 
        transform.SetLocalPositionAndRotation(_position, _rotation);
}
