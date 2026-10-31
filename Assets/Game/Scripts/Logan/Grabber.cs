using System;
using EMILtools.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [Required] public Interactor interactor;
    [Required] public FixedJoint joint;
    [ReadOnly] public Rigidbody grabbedRb;
    public float breakVal = 4;

    
    public void Grab(Rigidbody rigidbody)
    {
        Debug.Log("Grabbed : " + rigidbody.name + "");
        joint.connectedBody = rigidbody;   
        grabbedRb = rigidbody;
    }

    public void StopGrab()
    {
        if (grabbedRb == null) return;
        
        joint.connectedBody = null;
        if (grabbedRb != null) grabbedRb.isKinematic = false;
        grabbedRb = null;
    }

    void OnJointBreak(float breakForce)
    {
        if (grabbedRb != null) grabbedRb.isKinematic = false;
        grabbedRb = null;
        joint = gameObject.AddComponent<FixedJoint>();
        joint.breakForce = breakVal;
        joint.breakTorque = breakVal;
    }
}
