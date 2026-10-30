using System;
using EMILtools.Extensions;
using UnityEditor;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] public GameObject _camera;
    [SerializeField] public float _interactDistance = 5f;

    public Action<Rigidbody> OnGrab;
    public Action OnStopInteraction;

    void Update()
    {
        CheckInteraction();
        CheckStopInteraction();
    }

    void CheckInteraction()
    {
        //if (Input.GetMouseButtonDown(0))
        if (!Input.GetMouseButtonDown(0)) return;
        
        Debug.Log("Click!");

        if (!Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactDistance)) return;
        
        if(hit.collider.gameObject.TryGetComponent<IInteractible>(out var interactible))
        {
            Debug.Log(hit.collider.gameObject.name);
            interactible.Interact();
            
            // TEMP CODE
            if (hit.collider.gameObject.Has(out Rigidbody rb))
                OnGrab?.Invoke(rb);
        }
        else
            Debug.Log("NO!");
    }

    void CheckStopInteraction()
    {
        if(!Input.GetMouseButtonUp(0)) return;
        OnStopInteraction?.Invoke();
    }
        
}