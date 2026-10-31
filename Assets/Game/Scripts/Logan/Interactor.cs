using System;
using EMILtools.Extensions;
using UnityEditor;
using UnityEngine;
using TMPro;

public class Interactor : MonoBehaviour
{
    [SerializeField] public GameObject _camera;
    [SerializeField] public float _interactDistance = 5f;
    [SerializeField] TextMeshProUGUI _textbox;
    public LayerMask interactionMask;

    
    public bool holdingDownMouse;
    public Action<Rigidbody> OnGrab;
    public Action OnStopInteraction;
    
    
    void Update()
    {
        CheckInteraction();
        CheckStopInteraction();
    }

    void CheckInteraction()
    {
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward, Color.red);
        _textbox.gameObject.SetActive(false);
        if (!Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactDistance, interactionMask))
            return;
        if (!hit.collider.gameObject.TryGetComponent<IInteractible>(out var interactible)) return;
        var ctx = interactible.GetInteractionCtx();
        
        if(!holdingDownMouse) _textbox.gameObject.SetActive(true);
        _textbox.SetText(ctx.interactionPrompt); //All Interactables Have interaction prompts, all interactables get it from different places
        
        if (!Input.GetMouseButtonDown(0)) return;
        
        holdingDownMouse = true;
        
        switch (interactible) //All interactables have different mutations
        {
            case Pickup pickup: 
                OnGrab?.Invoke(pickup._rb);
                pickup._rb.ResetVel3D();
                break;
            case Cookware cookware:  
                
                break;
        }
    }

    void CheckStopInteraction()
    {
        if(!Input.GetMouseButtonUp(0)) return;
        holdingDownMouse = false;
        OnStopInteraction?.Invoke();
    }
        
}