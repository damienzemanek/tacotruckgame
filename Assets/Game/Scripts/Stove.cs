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
    public float burnTimer = 0;
    public float timeToBurn = 15;
    public bool isCooked = false;
    public float cookSpeed = 0.02f;
    public bool isCooking = false;
    public bool hasItemOnStove;
    public bool resetting;

    void Start()
    {
        stoveEntryLoc.onHasAllIngredients += PutItemOnStove;
        isCooking = false;
    }

    public void StartStove()
    {
        isCooking = true;
        if(hasItemOnStove && !resetting)
            cookwareProgressSlider.gameObject.SetActive(true);
    }

    public void StopStove()
    {
        isCooking = false;
        cookwareProgressSlider.gameObject.SetActive(false);
        if (isCooked)
        {
            ProduceOutcome();
            isCooked = false;
        }
    }
    
    public void PutItemOnStove()
    {
        if (resetting) return;
        var item = stoveEntryLoc.ingredientProgress[0].ingredientGO;
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        if(item.Has(out Collider c)) c.enabled = false;
        itemOnStove = item;
        hasItemOnStove = true;
        Debug.Log("Put Item On");
    }
    
    public void StopResetTime() => resetting = false;
    
    
    void FixedUpdate()
    {
        // For logan: Progress the cookware, if the stove is cooking, has an item, and is not resseting,
        // it goes over, produce the burnt food outcome instead of the regular one (the regular one is in the parent named 'cookwareOutcome')
        if(!stoveEntryLoc.hasAllIngredients) return;
        if (!hasItemOnStove) return;
        if (!isCooking) return;
        if (resetting) return;
        Debug.Log("Stove: Time Remaining: " + cookwareProgress);
        isCooked = ProgressCookwareDelayedOutcome(-cookSpeed);
        if (isCooked)
        {
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
        Instantiate(cookwareOutcome.Pr_pickup, cookwareOutcomeCreationPosition.position, Quaternion.identity);
        ResetProgress();
        itemOnStove = null;
        Destroy(itemOnStove);
        hasItemOnStove = false;
        isCooking = false;
        stoveEntryLoc.ResetProgress();
        stoveEntryLoc.hasAllIngredients = false;
        
        resetting = true;
        Invoke(nameof(StopResetTime), 0.5f);
        Debug.Log("Meat Grinder: Produced meat");

    }
}
