using System;
using EMILtools.Systems;
using UnityEngine;

[Serializable]
public struct InteractionEventCtx
{
    public GameObject _gameObject;
    public Interactor _interactor;
    public string interactionPrompt;
}

public abstract class Interactible : MonoBehaviour
{
    public abstract InteractionEventCtx GetInteractionCtx(Interactor withInteractor);
    
    public Sub<InteractionEventCtx> SubOnInteract;
    public Sub<InteractionEventCtx> SubOnStopInteract;

    protected virtual void Awake()
    {
        SubOnInteract = new(OnInteract);
        SubOnStopInteract = new(OnStopInteract);
    }

    public virtual bool OnInteract(InteractionEventCtx ctx)
    {
        //Debug.Log("[INTERACTABLE] OnInteract");
        return true;
    }
    
    public virtual bool OnStopInteract(InteractionEventCtx ctx)
    {
        // unsubscribes
        //Debug.Log("[INTERACTABLE] OnStopInteract");
        ctx._interactor.OnStartInteraction.Remove(SubOnInteract);
        ctx._interactor.OnStopInteraction.Remove(SubOnStopInteract);
        return true;
    }
}

