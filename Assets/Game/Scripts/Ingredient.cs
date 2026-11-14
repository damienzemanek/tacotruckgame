using System;
using System.Linq;
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
                .TryGetCombination(out var comb, pickup._pickupSO, collidedIngredient.pickup._pickupSO)) return;
        
        if(collidedIngredient.outcomeDetermined || outcomeDetermined) return;
        outcomeDetermined = true;
        collidedIngredient.outcomeDetermined = true;
        
        Vector3 midpoint = (transform.position + collidedIngredient.transform.position) / 2f;
        Instantiate(comb.result.Pr_pickup, midpoint, Quaternion.identity);
        Debug.Log("[INGREDIENT] Ingredients combined: " + pickup._pickupSO + " + " + collidedIngredient.pickup._pickupSO + " = " + comb.result.Pr_pickup.name);

        var otherPickup = comb.combinations.FirstOrDefault(c => c.ingredientSO == pickup._pickupSO);
        if (otherPickup != null) Destroy(gameObject);
        
        var myPickup = comb.combinations.FirstOrDefault(c => c.ingredientSO == pickup._pickupSO);
        if (myPickup != null) Destroy(gameObject);
    }
}
