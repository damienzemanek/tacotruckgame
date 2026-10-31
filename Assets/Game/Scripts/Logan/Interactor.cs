using System;
using EMILtools.Extensions;
using EMILtools.Systems;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using TMPro;

public class Interactor : MonoBehaviour
{
    [SerializeField] public GameObject _camera;
    [SerializeField] public float _interactDistance = 5f;
    [SerializeField] TextMeshProUGUI _textbox;
    [Required] public Grabber grabber;
    public LayerMask interactionMask;

    public bool holdingDownMouse;
    public Publisher<InteractionEventCtx> OnStartInteraction = new();
    public Publisher<InteractionEventCtx> OnStopInteraction = new();

    
    
    void Update()
    {
        CheckInteraction();
        CheckStopInteraction();
    }

    void CheckInteraction()
    {
        // Defence and Setup Mutations
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward, Color.red);
        _textbox.gameObject.SetActive(false);
        
        if (!Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactDistance, interactionMask))
            return;
        if (!hit.collider.gameObject.TryGetComponent<Interactible>(out var interactible)) return;
        
        var ctx = interactible.GetInteractionCtx(withInteractor: this);
        
        if(!holdingDownMouse) _textbox.gameObject.SetActive(true);
        
        _textbox.SetText(ctx.interactionPrompt); //All Interactables Have interaction prompts, all interactables get it from different places
        
        if (!Input.GetMouseButtonDown(0)) return;
        
        OnStartInteraction.Add(interactible.SubOnInteract);
        OnStopInteraction.Add(interactible.SubOnStopInteract);
        OnStartInteraction.Publish(ctx);
        holdingDownMouse = true;
        
        Debug.Log("Interactor: Started interacting with " + (interactible));
        
        switch (interactible) //All interactables have different mutations
        {
            case Pickup pickup: 
                Debug.Log("Interactor: Pickup Interaction");

                
                grabber.Grab(pickup._rb);
                pickup._rb.ResetVel3D();
                break;
            case Cookware cookware:  
                Debug.Log("Interactor: Cookware Interaction");
                
                
                break;
            default:
                Debug.LogError($"No matching interaction type: {interactible.GetType().Name}");
                break;

        }
    }

    void CheckStopInteraction()
    {
        if(!Input.GetMouseButtonUp(0)) return;
        grabber.StopGrab();
        holdingDownMouse = false;
        OnStopInteraction.Publish(new() { _interactor = this}); // struct so not expensive
    }
        
}