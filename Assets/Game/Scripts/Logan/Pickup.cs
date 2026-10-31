using UnityEngine;

public class Pickup : Item
{
    public override IInteractible.ItemContext Interact()
    {
        _itemContext._rb = gameObject.GetComponent<Rigidbody>();
        return base.Interact();
    }
}
