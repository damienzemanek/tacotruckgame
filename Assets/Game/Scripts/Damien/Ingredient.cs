using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public Pickup pickup;
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Test");
        if (!collision.gameObject.TryGetComponent(out Ingredient collidedIngredient)) return;
        Debug.Log("hit Col");
        if (!PersistentConfigurationDataHolder.Instance.ingredientCombinationDb
                .TryGetResult(out var result, pickup._pickupSO, collidedIngredient.pickup._pickupSO)) return;

        Debug.Log(result.name);
        collidedIngredient.gameObject.SetActive(false);
        Destroy(collidedIngredient);
        
        Destroy(gameObject);
    }
}
