using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Cookware : MonoBehaviour, IInteractible
{
  [SerializeField] TextMeshProUGUI _textbox;
    void IInteractible.Interact()
    {
      Debug.Log("cookware");
      _textbox.SetText("E to interact with: " + "cookware");
    }
}