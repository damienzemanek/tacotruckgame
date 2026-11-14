using System;
using EMILtools.Systems;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [Required] public FixedJoint joint;
    [ReadOnly] public Rigidbody grabbedRb;
    public float releaseForce = 1.25f;
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
        
        joint.connectedBody.AddForce(Vector3.down * releaseForce, ForceMode.Impulse);
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
