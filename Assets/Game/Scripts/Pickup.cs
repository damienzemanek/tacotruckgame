using System;
using System.Collections.Generic;
using EMILtools.Systems;
using UnityEngine;

public class Pickup : Interactible
{
    public PickupSO _pickupSO;
    public Rigidbody _rb;
    [SerializeReference] public List<PickupTrait> traits = new();

    private void OnEnable()
    {
        foreach (var t in traits) t.ResetTrait();
    }

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


[Serializable]
public abstract class PickupTrait
{
    public abstract string traitName { get; }
    public abstract void ResetTrait();
}

[Serializable]
public class BurnableTrait : PickupTrait
{
    public override string traitName => "Burnable";
    public float burnTimer = 0;
    public float burnMaxTime = 10f;
    public override void ResetTrait() => burnTimer = burnMaxTime;
}