using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Cookware : MonoBehaviour, IInteractible
{
  [SerializeField] TextMeshProUGUI _textbox;
    public IInteractible.ItemContext Interact()
    {
      IInteractible.ItemContext _itemContext = new();
      Debug.Log("cookware");
      _textbox.SetText("E to interact with: " + "cookware");
      return _itemContext;
    }
}