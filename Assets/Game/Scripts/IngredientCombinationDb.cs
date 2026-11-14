using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientCombinationDb", menuName = "SO/Ingredient Combination Db")]
public class IngredientCombinationDb : ScriptableObject
{

    [Serializable]
    public class IngredientMutationConfig
    {
        public PickupSO ingredientSO;
        public bool consumedWhenCombined;
    }
    
    [Serializable]
    public class IngredientCombinationResult
    {
        public int size => combinations.Count;
        public List<IngredientMutationConfig> combinations;
        public PickupSO result;
    }

    [SerializeReference] public List<IngredientCombinationResult> db;

    public bool TryGetCombination(out IngredientCombinationResult comb, params PickupSO[] ingredients)
    {
        comb = null;
        
        foreach (IngredientCombinationResult combination in db)
        {
            var matches = 0;
            foreach (var ing in ingredients)
            {
                if(combination.combinations.Any(imc => imc.ingredientSO == ing)) matches++;
                if (matches < combination.size) continue;
                comb = combination;
                return true;
            }
        }
        return false;
    }
    
    public bool TryGetResult(out PickupSO result, params PickupSO[] ingredients)
    {
        result = null;
        
        foreach (IngredientCombinationResult combination in db)
        {
            var matches = 0;
            foreach (var ing in ingredients)
            {
                if(combination.combinations.Any(imc => imc.ingredientSO == ing)) matches++;
                if (matches < combination.size) continue;
                result = combination.result;
                return true;
            }
        }
        return false;
    }
}