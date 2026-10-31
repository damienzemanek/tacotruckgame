using UnityEngine;

public class Pickup : Item
{
    public Rigidbody _rb;
    public override IInteractible.ItemContext Interact()
    {
        var ctx = base.Interact();
        Debug.Log("Pickup");
        ctx._rb = _rb;
        return ctx;
    }
}
