using UnityEngine;
using TMPro;

public class Item : MonoBehaviour, IInteractible
{
    [SerializeField] TextMeshProUGUI _textbox;
    void IInteractible.Interact()
    {
      Debug.Log("item");
       _textbox.SetText("E to interact with: " + "item");
    }
}