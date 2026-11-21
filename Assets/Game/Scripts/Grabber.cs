using System;
using DG.Tweening;
using EMILtools.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [ReadOnly] public Rigidbody grabbedRb;
    public Transform grabPoint;
    public float releaseForce = 0.5f;
    public float moveDuration = 0.5f;
    public bool grabbed = false;
    private Sequence grab;

    private Vector3 grabVel;
    public float grabSmoothingTime = 0.01f;
    
    private void FixedUpdate()
    {
        if(!grabbed) return;
        if(grabbedRb == null) return;
        grabbedRb.transform.position = Vector3.SmoothDamp(
            grabbedRb.transform.position,
            grabPoint.position,
            ref grabVel,
            grabSmoothingTime);
    }

    public void Grab(Rigidbody rigidbody)
    {
        if(grabbedRb != null) return;
        grab = DOTween.Sequence();
        Debug.Log("Grabbed : " + rigidbody.name + "");
        rigidbody.transform.SetParent(grabPoint);
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        grab.Append(rigidbody.transform.DOLocalMove(Vector3.zero, moveDuration));
        grab.OnComplete(() => grabbed = true);
        grabbedRb = rigidbody;
    }

    public void StopGrab()
    {
        if (grabbedRb == null) return;
        grab.Kill();
        grabbedRb.transform.SetParent(null);
        grabbedRb.ResetVel3D();
        grabbedRb.isKinematic = false;
        grabbedRb.useGravity = true;
        grabbed = false;
        //grabbedRb.AddForce(Vector3.down * releaseForce, ForceMode.Impulse);
        grabbedRb = null;
    }
}
