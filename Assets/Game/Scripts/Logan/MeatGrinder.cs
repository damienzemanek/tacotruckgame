using System;
using UnityEngine;

public class MeatGrinder : Cookware
{
    public CookwareIngredientCombinationLoc meatEntryLoc;
    const float MEAT_AMOUNT = 100;
    public float meatUsageSpeed = 1f;
    public bool hasMeatAlready;
    public bool isMouseHeldAndMeatIsGrinding;
    public bool resetting;
    [Range(0, 100)] public float currentRemainingMeatAmount;

    protected override void Awake()
    {
        meatEntryLoc.onHasAllIngredients += PutMeatInGrinder;
        base.Awake();
    }
    
    public void PutMeatInGrinder()
    {
        if (resetting) return;
        currentRemainingMeatAmount = MEAT_AMOUNT;
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
        currentRemainingMeatAmount -= meatUsageSpeed * Time.deltaTime;
        Debug.Log("Meat Grinder: Meat Remaining: " + currentRemainingMeatAmount);
        if(currentRemainingMeatAmount <= 0) ProduceOutcome();
    }

    public override bool OnInteract(InteractionEventCtx ctx)
    {
        Debug.Log("Meat Grinder: Started using");
        isMouseHeldAndMeatIsGrinding = true;
        return base.OnInteract(ctx);
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
        currentRemainingMeatAmount = 0;
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