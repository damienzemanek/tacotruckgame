using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public Pickup pickup;
    public bool outcomeDetermined;
    
    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out Ingredient collidedIngredient)) return;
        if (!PersistentConfigurationDataHolder.Instance.ingredientCombinationDb
                .TryGetResult(out var result, pickup._pickupSO, collidedIngredient.pickup._pickupSO)) return;
        
        if(collidedIngredient.outcomeDetermined || outcomeDetermined) return;
        outcomeDetermined = true;
        collidedIngredient.outcomeDetermined = true;
        
        collidedIngredient.gameObject.SetActive(false);
        Vector3 midpoint = (transform.position + collidedIngredient.transform.position) / 2f;
        Instantiate(result.Pr_pickup, midpoint, Quaternion.identity);
        Debug.Log("[INGREDIENT] Ingredients combined: " + pickup._pickupSO + " + " + collidedIngredient.pickup._pickupSO + " = " + result.Pr_pickup.name);
        Destroy(gameObject);
    }
}
