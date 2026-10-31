using System;
using EMILtools.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public abstract class Cookware : Interactible
{
    [SerializeField] protected Transform cookwareOutcomeCreationPosition;
    [SerializeField] protected PickupSO cookwareOutcome;
    [SerializeField] public CookwareSO _cookwareSO;
    
    public override InteractionEventCtx GetInteractionCtx(Interactor withInteractor) => new()
    {
        _gameObject = gameObject,
        interactionPrompt = _cookwareSO.cookwarePrompt,
        _interactor = withInteractor,
    };

    public override bool OnInteract(InteractionEventCtx ctx)
    {
        Debug.Log("[COOKWARE]: OnInteract Called");
        return base.OnInteract(ctx);
    }
    
    public abstract void ProduceOutcome(); // All cookware will have an outcome associated with their usage
}