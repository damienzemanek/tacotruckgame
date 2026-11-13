using Sirenix.OdinInspector;
using UnityEngine;


/// <summary>
/// THIS IS PUT ON THE STOVE BUTTON TO START THE STOVE
/// </summary>
public class Stove : Cookware
{
    [Required] public Transform stoveReparentPos;
    [Required] public GameObject itemOnStove;
    [Required] public CookwareIngredientCombinationLoc stoveEntryLoc;
    [SerializeField, Required] protected PickupSO burntFoodOutcome;
    public float cookSpeed = 0.02f;
    public bool isCooking;
    public bool hasItemOnStove;
    public bool resetting;
    
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
    }
    
    public void PutItemOnStove(GameObject item)
    {
        if (resetting) return;
        item.transform.SetParent(stoveReparentPos);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
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
        PutItemOnStove(itemOnStove);
        if (!hasItemOnStove) return;
        if (!isCooking) return;
        if (resetting) return;
        ProgressCookware(-cookSpeed);
        Debug.Log("Stove: Time Remaining: " + cookwareProgress);
        if (itemOnStove == cookwareOutcome.Pr_pickup)
        {
            cookwareOutcome = burntFoodOutcome;
            ProgressCookware(-cookSpeed);
        }
        else
        {
            ProgressCookware(-cookSpeed);
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
        hasItemOnStove = false;
        isCooking = false;
        stoveEntryLoc.ResetProgress();
        stoveEntryLoc.hasAllIngredients = false;
        
        resetting = true;
        Invoke(nameof(StopResetTime), 0.5f);
        Debug.Log("Meat Grinder: Produced meat");

    }
}
