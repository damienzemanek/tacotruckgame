using System;
using EMILtools.Systems;
using UnityEngine;

public class Pickup : Interactible
{
    public PickupSO _pickupSO;
    public Rigidbody _rb;
    
    public override InteractionEventCtx GetInteractionCtx(Interactor withInteractor) => new()
    {
        _gameObject = gameObject,
        interactionPrompt = _pickupSO.pickupPrompt,
        _interactor = withInteractor,
    };

    public override bool OnInteract(InteractionEventCtx ctx)
    {
        Debug.Log("[PICKUP]: OnInteract Called");
        return base.OnInteract(ctx); // No op
    }
}
