using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Customer : Interactible
{
    OrderTracker orders => OrderTracker.Instance;
    
    public int myIndex;
    [ReadOnly] public CustomersConfig config;
    public Vector3 walkAwayPositionOffset;
    public float walkAwayDuration;
    public CustomerStates state = CustomerStates.AvaliableToTakeOrder;
    
    public enum CustomerStates
    {
        AvaliableToTakeOrder,
        WaitingOnFood,
        Done,
    }
    
    public void AvaliableToTakeOrder()
    {
        state = CustomerStates.AvaliableToTakeOrder;
    }

    public void OrderTaken()
    {
        state = CustomerStates.WaitingOnFood;
    }
    
    public void GivenFood()
    {
        state = CustomerStates.Done;
        transform.DOLocalMove(transform.position + walkAwayPositionOffset, walkAwayDuration);
    }

    public override InteractionEventCtx GetInteractionCtx(Interactor withInteractor)
    {
        return new InteractionEventCtx()
        {
            _gameObject = gameObject,
            _interactor = withInteractor,
            interactionPrompt = GetInteractionPrompt()
        };
    }

    string GetInteractionPrompt()
    {
        switch (state)
        {
            case CustomerStates.AvaliableToTakeOrder: return "Take Order";
            case CustomerStates.Done: return string.Empty;
            case CustomerStates.WaitingOnFood: return orders.placementZone.hasAllIngredients ? "Give Food" : string.Empty;
        }
        return string.Empty;
    }
}