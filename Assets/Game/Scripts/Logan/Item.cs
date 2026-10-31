using UnityEngine;
using TMPro;

public class Item : MonoBehaviour, IInteractible
{
    public ItemSO _itemSO;
    //[SerializeField] TextMeshProUGUI _textbox;
    public virtual IInteractible.ItemContext Interact()
    {
    IInteractible.ItemContext _itemContext = new();
    _itemContext._gameObject = 
     // _itemContext._rb = gameObject.GetComponent<Rigidbody>();
        return _itemContext;
    }
}