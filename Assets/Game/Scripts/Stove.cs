using System;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;


/// <summary>
/// THIS IS PUT ON THE STOVE BUTTON TO START THE STOVE
/// </summary>
public class Stove : Cookware
{
    [ReadOnly] public GameObject itemOnStove;
    [Required] public CookwareIngredientCombinationLoc stoveEntryLoc;
    [SerializeField, Required] protected PickupSO burntFoodOutcome;
    [SerializeField] GameObject go_FireEff;
    public float burnTimer = 0;
    public float timeToBurn = 15;
    public bool isCooked = false;
    public float cookSpeed = 0.02f;
    public bool isCooking = false;
    public bool resetting;

    
    void Start()
    {
        StopShowingInfoText();
        stoveEntryLoc.onHasAllIngredients += PutItemOnStove;
        stoveEntryLoc.onNoLongerHasAllIngredients += RemoveItemFromStove;
        isCooking = false;
        go_FireEff.gameObject.SetActive(false);
    }

    void ResetStove()
    {
        isCooking = false;
        go_FireEff.gameObject.SetActive(false);
        StopShowingInfoText();
        cookwareProgressSlider.gameObject.SetActive(false);
    }

    public void StartStove()
    {
        isCooking = true;
        go_FireEff.gameObject.SetActive(true);
        if(itemOnStove != null && !resetting)
            cookwareProgressSlider.gameObject.SetActive(true);
    }

    public void StopStove()
    {
        ResetStove();
        if(itemOnStove && !isCooked)
            ShowInfoText("Ready!");
        if (!isCooked || !itemOnStove) return;
        ProduceOutcome();
        isCooked = false;
    }
    
    public void PutItemOnStove()
    {
        if (resetting) return;
        var item = stoveEntryLoc.ingredientProgress[0].ingredientGO;
        itemOnStove = item;
        Debug.Log("Put Item On");
        ShowInfoText("Ready!");
        burnTimer = timeToBurn;
    }

    public void RemoveItemFromStove()
    {
        ResetStove();
        itemOnStove = null;
        Debug.Log("Remove Item From Stove");
    }
    
    public void StopResetTime() => resetting = false;
    
    
    void FixedUpdate()
    {
        // For logan: Progress the cookware, if the stove is cooking, has an item, and is not resseting,
        // it goes over, produce the burnt food outcome instead of the regular one (the regular one is in the parent named 'cookwareOutcome')
        if(!stoveEntryLoc.hasAllIngredients) return;
        if (itemOnStove == null) return;
        if (!isCooking) return;
        if (resetting) return;
        Debug.Log("Stove: Time Remaining: " + cookwareProgress);
        isCooked = ProgressCookwareDelayedOutcome(-cookSpeed);
        if (isCooked)
        {
            ShowInfoText("!");
            burnTimer -= Time.deltaTime;
            if (burnTimer <= 0)
                cookwareOutcome = burntFoodOutcome;
        }
    }

    public override bool OnInteract(InteractionEventCtx ctx)
    {
        base.OnInteract(ctx);
        StartStove();
        return true;
    }

    public override void ProduceOutcome()
    {
        StopShowingInfoText();
        Instantiate(cookwareOutcome.Pr_pickup, cookwareOutcomeCreationPosition.position, Quaternion.identity);
        ResetProgress();
        Destroy(itemOnStove);
        itemOnStove = null;
        isCooking = false;
        stoveEntryLoc.ResetProgress();
        stoveEntryLoc.hasAllIngredients = false;
        
        resetting = true;
        Invoke(nameof(StopResetTime), 0.5f);
        Debug.Log("Stove: Outcome Produced");

    }
}
