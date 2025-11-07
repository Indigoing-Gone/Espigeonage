using UnityEngine;

public class Draggable : MonoBehaviour, IDraggable
{
    private Rigidbody rb;
    private ConfigurableJoint joint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Drag(Dragger _dragger, Rigidbody _dragPointRb)
    {
        //Update Rigidbody
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        //rb.useGravity = false;


        //Generate Joint
        joint = rb.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = _dragPointRb;

        //Set Anchors
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = new(0, 0, 0);
        joint.connectedAnchor = new(0, 0, 0);

        //Set Motion
        joint.xMotion = joint.yMotion = joint.zMotion = ConfigurableJointMotion.Locked;
        joint.angularXMotion = joint.angularYMotion = joint.angularZMotion = ConfigurableJointMotion.Locked;
    }

    public void Release()
    {
        if (joint) Destroy(joint);
        //rb.useGravity = true;
    }
}
