using System;
using System.Collections.Generic;
using System.Linq;
using EMILtools.Extensions;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public Pickup pickup;
    public bool outcomeDetermined;
    
    public List<PickupTrait> traits = new List<PickupTrait>();

    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out Ingredient collidedIngredient)) return;
        if (!PersistentConfigurationDataHolder.Instance.ingredientCombinationDb
                .TryGetCombination(out var comb, pickup._pickupSO, collidedIngredient.pickup._pickupSO)) return;
        
        Debug.Assert(comb != null, "DB should've pulled the comb down");

        
        if(collidedIngredient.outcomeDetermined || outcomeDetermined) return;
        outcomeDetermined = true;
        collidedIngredient.outcomeDetermined = true;
        
        Vector3 midpoint = (transform.position + collidedIngredient.transform.position) / 2f;
        Instantiate(comb.result.Pr_pickup, midpoint, Quaternion.identity);
        Debug.Log("[INGREDIENT] Ingredients combined: " + pickup._pickupSO + " + " + collidedIngredient.pickup._pickupSO + " = " + comb.result.Pr_pickup.name);

        var otherPickup = comb.combinations.FirstOrDefault(c => c.ingredientSO.pickupName == collidedIngredient.pickup._pickupSO.pickupName);
        if (otherPickup != null && otherPickup.consumedWhenCombined) Destroy(collidedIngredient.pickup.gameObject);
        
        var myPickup = comb.combinations.FirstOrDefault(c => c.ingredientSO.pickupName == pickup._pickupSO.pickupName);
        if (myPickup != null && myPickup.consumedWhenCombined) Destroy(pickup.gameObject);
    }
}
