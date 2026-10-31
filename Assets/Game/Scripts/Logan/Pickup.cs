using UnityEngine;

public class Pickup : MonoBehaviour, IInteractible
{
    public PickupSO _pickupSO;
    public Rigidbody _rb;
    public InteractionEventCtx GetInteractionCtx() => new()
    {
        _gameObject = gameObject,
        interactionPrompt = _pickupSO.pickupPrompt,
    };
}
