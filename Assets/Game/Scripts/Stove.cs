using System;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;


/// <summary>
/// THIS IS PUT ON THE STOVE BUTTON TO START THE STOVE
/// </summary>
public class Stove : Cookware
{
    [ReadOnly] public Ingredient itemOnStove;
    [Required] public CookwareIngredientCombinationLoc stoveEntryLoc;
    [SerializeField, Required] protected PickupSO burntFoodOutcome;
    [SerializeField] GameObject go_FireEff;
    public bool isCooked = false;
    public bool isCooking = false;
    public bool resetting;

    GameObject cachedToDeleteItemOnStove = null;
    [ReadOnly, SerializeReference] public BurnableTrait burntTraitReference = null;
    
    void Start()
    {
        StopShowingInfoText();
        stoveEntryLoc.onHasAllIngredients += PutItemOnStove;
        stoveEntryLoc.onNoLongerHasAllIngredients += RemoveItemFromStove;
        isCooking = false;
        go_FireEff.gameObject.SetActive(false);
        burntTraitReference = null;
    }



    public void StartStove()
    {
        isCooking = true;
        go_FireEff.gameObject.SetActive(true);
        if (itemOnStove == null || resetting) return;
        cookwareProgressSlider.gameObject.SetActive(true);
        StopShowingInfoText();
    }

    public void StopStove()
    {
        isCooking = false;
        go_FireEff.gameObject.SetActive(false);
        StopShowingInfoText();
        cookwareProgressSlider.gameObject.SetActive(false);
        
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
        itemOnStove = item.Get<Ingredient>();
        burntTraitReference = (BurnableTrait)itemOnStove.pickup.traits.Find(t => t.traitName == "Burnable");
        cookableTrait = (CookableTrait)itemOnStove.pickup.traits.Find(t => t.traitName == "Cookable");
        Debug.Log("Put Item On");
        if (isCooking)
            StartStove();
        else
            ShowInfoText("Ready!");

    }

    public void RemoveItemFromStove()
    {
        cookwareProgressSlider.gameObject.SetActive(false);
        StopShowingInfoText();
        itemOnStove = null;
        burntTraitReference = null;
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
        isCooked = ProgressCookwareDelayedOutcome();
        if (isCooked)
        {
            cachedToDeleteItemOnStove = itemOnStove.gameObject;
            cookwareProgressSlider.gameObject.SetActive(false);
            ShowInfoText("Food Burning!");
            burntTraitReference.burnTimer -= Time.deltaTime;
            if (burntTraitReference.burnTimer <= 0)
            {
                cookwareOutcome = burntFoodOutcome;
                ProduceOutcome();
                isCooked = false;
            }
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
        Destroy(cachedToDeleteItemOnStove);
        cachedToDeleteItemOnStove = null;
        itemOnStove = null;
        burntTraitReference = null;
        stoveEntryLoc.ResetProgress();
        stoveEntryLoc.hasAllIngredients = false;
        
        resetting = true;
        Invoke(nameof(StopResetTime), 0.5f);
        Debug.Log("Stove: Outcome Produced");

    }
}
