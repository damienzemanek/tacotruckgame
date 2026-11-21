using System;
using EMILtools.Extensions;
using UnityEngine;

public sealed class MeatGrinder : Cookware
{
    public CookwareIngredientCombinationLoc meatEntryLoc;
    public bool hasMeatAlready;
    public bool isMouseHeldAndMeatIsGrinding;
    public bool resetting;

    protected override void Awake()
    {
        meatEntryLoc.onHasAllIngredients += PutMeatInGrinder;
        base.Awake();
    }

    private void Start()
    {
        cookableTrait = null;
        StopShowingInfoText();
    }

    public void PutMeatInGrinder()
    {
        if (resetting) return;
        ResetProgress();
        hasMeatAlready = true;
        meatEntryLoc.hasAllIngredients = false;
        meatEntryLoc.SetPreventIngredientEntries(true);
        cookableTrait = (CookableTrait)meatEntryLoc.ingredientProgress[0].ingredientGO.
            Get<Ingredient>().pickup.traits
            .Find(t => t.traitName == "Cookable");
        ShowInfoText("Ready!");
        Debug.Log("Meat Grinder: Put meat in grinder");
    }
    
    public void StopResetTime() => resetting = false;
    

    // Meat Grinder: GRINDS THE MEAT
    // CALLED IN FIXED TIME POLL
    void FixedUpdate()
    {
        if (!hasMeatAlready) return;
        if (!isMouseHeldAndMeatIsGrinding) return;
        if (resetting) return;
        ProgressCookwareLinear();
        Debug.Log("Meat Grinder: Meat Remaining: " + cookwareProgress);
    }

    public override bool OnInteract(InteractionEventCtx ctx)
    {
        if (resetting) return false;
        Debug.Log("Meat Grinder: Started using");
        base.OnInteract(ctx);
        isMouseHeldAndMeatIsGrinding = true;
        cookwareProgressSlider.gameObject.SetActive(hasMeatAlready);
        StopShowingInfoText();
        return true;
    }

    public override bool OnStopInteract(InteractionEventCtx ctx)
    {
        InternlOnStopInteractMeatGrinder();
        return base.OnStopInteract(ctx);
    }

    void InternlOnStopInteractMeatGrinder()
    {
        Debug.Log("Meat Grinder: Stopped using");
        isMouseHeldAndMeatIsGrinding = false;
        cookwareProgressSlider.gameObject.SetActive(false);
        if(hasMeatAlready)
            ShowInfoText("Ready!");
        else
            StopShowingInfoText();
    }
    

    public override void ProduceOutcome()
    {
        Instantiate(cookwareOutcome.Pr_pickup, cookwareOutcomeCreationPosition.position, Quaternion.identity);
        ResetProgress();
        isMouseHeldAndMeatIsGrinding = false;
        meatEntryLoc.SetPreventIngredientEntries(false);
        hasMeatAlready = false;
        meatEntryLoc.ResetProgress();
        meatEntryLoc.hasAllIngredients = false;
        resetting = true;
        cookableTrait = null;
        InternlOnStopInteractMeatGrinder();
        Invoke(nameof(StopResetTime), 0.5f);
        Debug.Log("Meat Grinder: Produced meat");

    }


}