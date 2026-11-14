using System;
using EMILtools.Systems;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public abstract class Cookware : Interactible
{
    [ReadOnly] public float cookwareProgress => cookwareProgressSlider.value;
    [SerializeField, Required] public SliderWorld cookwareProgressSlider;
    [SerializeField, Required] protected Transform cookwareOutcomeCreationPosition;
    [SerializeField, Required] protected PickupSO cookwareOutcome;
    [SerializeField, Required] public CookwareSO _cookwareSO;
    
    public override InteractionEventCtx GetInteractionCtx(Interactor withInteractor) => new()
    {
        _gameObject = gameObject,
        interactionPrompt = _cookwareSO.cookwarePrompt,
        _interactor = withInteractor,
    };
    
    protected override void Awake()
    {
        base.Awake(); 
        ResetProgress();
        cookwareProgressSlider.gameObject.SetActive(false);
    }

    public override bool OnInteract(InteractionEventCtx ctx)
    {
        //Debug.Log("[COOKWARE]: OnInteract Called");
        base.OnInteract(ctx);
        return true;
    }
    
    public void ResetProgress() => cookwareProgressSlider.value = 1f;
    
    public bool ProgressCookwareLinear(float progressAmount)
    {
        cookwareProgressSlider.MutateValue(progressAmount);
        if (!cookwareProgressSlider.IsEmpty) return false;
        ProduceOutcome();
        return true;
    }

    public bool ProgressCookwareDelayedOutcome(float progressAmount)
    {
        cookwareProgressSlider.MutateValue(progressAmount);
        if (!cookwareProgressSlider.IsEmpty) return false;
        return true;
    }
    
    public abstract void ProduceOutcome(); // All cookware will have an outcome associated with their usage
}