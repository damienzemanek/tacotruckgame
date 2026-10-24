using UnityEngine;

public class Cookware : MonoBehaviour, IInteractible
{
    void IInteractible.Interact()
    {
      Debug.Log("cookware");
    }
}