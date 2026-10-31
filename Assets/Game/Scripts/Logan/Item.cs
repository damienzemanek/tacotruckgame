using UnityEngine;
using TMPro;

public class Item : MonoBehaviour, IInteractible
{
    [SerializeField] public IInteractible.ItemContext _itemContext;
    //[SerializeField] TextMeshProUGUI _textbox;
    public virtual IInteractible.ItemContext Interact()
    {
    IInteractible.ItemContext _itemContext = new();
      _itemContext._gameObject = gameObject;
     // _itemContext._rb = gameObject.GetComponent<Rigidbody>();
        return _itemContext;
    }
}