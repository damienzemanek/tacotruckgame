using System;
using UnityEngine;

public sealed class MeatGrinder : Cookware
{
    public CookwareIngredientCombinationLoc meatEntryLoc;
    public float meatUsageSpeed = 0.02f;
    public bool hasMeatAlready;
    public bool isMouseHeldAndMeatIsGrinding;
    public bool resetting;

    protected override void Awake()
    {
        meatEntryLoc.onHasAllIngredients += PutMeatInGrinder;
        base.Awake();
    }
    
    public void PutMeatInGrinder()
    {
        if (resetting) return;
        ResetProgress();
        hasMeatAlready = true;
        meatEntryLoc.hasAllIngredients = false;
        meatEntryLoc.SetPreventIngredientEntries(true);
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
        ProgressCookware(-meatUsageSpeed);
        Debug.Log("Meat Grinder: Meat Remaining: " + cookwareProgress);
    }

    public override bool OnInteract(InteractionEventCtx ctx)
    {
        Debug.Log("Meat Grinder: Started using");
        base.OnInteract(ctx);
        isMouseHeldAndMeatIsGrinding = true;
        if(!hasMeatAlready) cookwareProgressSlider.gameObject.SetActive(false);
        return true;
    }

    public override bool OnStopInteract(InteractionEventCtx ctx)
    {
        Debug.Log("Meat Grinder: Stopped using");
        isMouseHeldAndMeatIsGrinding = false;
        return base.OnStopInteract(ctx);
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
        Invoke(nameof(StopResetTime), 0.5f);
        Debug.Log("Meat Grinder: Produced meat");

    }


}