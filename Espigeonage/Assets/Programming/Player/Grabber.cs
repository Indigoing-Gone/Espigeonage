using System;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform grabLocation;

    private GrabbableObject currentGrabbedObject;

    public void GrabObject(GrabbableObject _grabbedObject)
    {
        if (currentGrabbedObject) return;
        currentGrabbedObject = _grabbedObject;
        currentGrabbedObject.transform.parent = grabLocation;
        currentGrabbedObject.transform.localPosition = Vector3.zero;
    }
}
