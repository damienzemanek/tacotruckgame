using Sirenix.OdinInspector;
using UnityEngine;

public class Stove : Cookware
{
    public CookwareIngredientCombinationLoc stopButton;
    [SerializeField, Required] protected PickupSO burntFoodOutcome;
    public float cookSpeed = 0.02f;
    public GameObject 
    public bool isCookingAlready;
    public bool resetting;

    protected override void Awake()
    {
        meatEntryLoc.onHasAllIngredients += PutMeatOnStove;
        base.Awake();
    }
    
    public void PutMeatOnStove()
    {
        if (resetting) return;
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
