using UnityEngine;

public class Item : MonoBehaviour, IInteractible
{
    void IInteractible.Interact()
    {
      Debug.Log("item");
    }
}