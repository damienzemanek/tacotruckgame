using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientCombinationDb", menuName = "SO/Ingredient Combination Db")]
public class IngredientCombinationDb : ScriptableObject
{
    
    [Serializable]
    public class IngredientCombinationResult
    {
        public int size => combinations.Count;
        public List<PickupSO> combinations;
        public PickupSO result;
    }
        
    [SerializeReference] public List<IngredientCombinationResult> db;
    public bool TryGetResult(out PickupSO result, params PickupSO[] ingredients)
    {
        result = null;
        
        foreach (IngredientCombinationResult combination in db)
        {
            var matches = 0;
            foreach (var ing in ingredients)
            {
                if(combination.combinations.Contains(ing)) matches++;
                if (matches >= combination.size)
                {
                    result = combination.result;
                    return true;
                }
            }
        }
        return false;
    }
}