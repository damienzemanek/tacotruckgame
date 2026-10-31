using UnityEngine;

public class Pickup : Item
{
    public Rigidbody _rb;
    public override IInteractible.ItemContext Interact()
    {
        var ctx = base.Interact();
        ctx._rb = _rb;
        return ctx;
    }
}
