using System;
using UnityEngine;

public class Grabbable : MonoBehaviour, IGrabbable
{
    private Collider col;
    public event Action BeingGrabbed;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    public bool Grab(Grabber _grabber)
    {
        if (_grabber == null) return false;

        BeingGrabbed?.Invoke();

        transform.parent = _grabber.GrabLocation;
        transform.localPosition = Vector3.zero;
        col.enabled = !_grabber.DisableGrabbableCollider;

        return true;
    }

    public void Release()
    {
        transform.parent = null;
        col.enabled = true;
    }
}
