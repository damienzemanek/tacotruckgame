using UnityEngine;
using TMPro;

public class Item : MonoBehaviour, IInteractible
{
    public ItemSO _itemSO;

    //[SerializeField] TextMeshProUGUI _textbox;
    public virtual IInteractible.ItemContext Interact()
    {
        Debug.Log("Item Interact");
        return new IInteractible.ItemContext()
        {
            _itemSOData = _itemSO,
            _gameObject = gameObject,
            _rb = null
        };
    }
}
