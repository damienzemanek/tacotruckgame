using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [Required] public Interactor interactor;
    [Required] public FixedJoint joint;

    public float breakVal = 4;

    void OnEnable()
    {
        interactor.OnGrab += Grab;
        interactor.OnStopInteraction += StopGrab;
    }

    void OnDisable()
    {
        interactor.OnGrab -= Grab;
        interactor.OnStopInteraction -= StopGrab;
    }
    
    void Grab(Rigidbody rigidbody)
    {
        Debug.Log("Grabbed : " + rigidbody.name + "");
        joint.connectedBody = rigidbody;   
    }

    void StopGrab()
    {
        joint.connectedBody = null;
    }

    void OnJointBreak(float breakForce)
    {
        joint = gameObject.AddComponent<FixedJoint>();
        joint.breakForce = breakVal;
        joint.breakTorque = breakVal;
    }
}
