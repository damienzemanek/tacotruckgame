using System;
using System.Collections.Generic;
using System.Linq;
using EMILtools.Extensions;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;

public class CookwareIngredientCombinationLoc : MonoBehaviour
{
    [Serializable]
    public struct IngredientCombinationProgression
    {
        public PickupSO ingredient;
        public GameObject ingredientGO;
        public bool hasIngredient;
    }

    public bool preventIngredientEntries;
    public bool hasAllIngredients;
    public Cookware cookwareParent;
    [SerializeField] public IngredientCombinationProgression[] ingredientProgress;
    [NonSerialized, ShowInInspector] public List<(Collider, int)> enteredIndiceCache = new();
    public Action onHasAllIngredients;
    public Action onNoLongerHasAllIngredients;
    

    void OnTriggerEnter(Collider other)
    {
        if (preventIngredientEntries) return;
        if (!other.TryGetComponent(out Ingredient ingredient)) return;
        int index = Array.FindIndex(ingredientProgress, p => p.ingredient == ingredient.pickup._pickupSO);
        if (index == -1) return;
        ref IngredientCombinationProgression progress = ref ingredientProgress[index];
        progress.ingredientGO = other.gameObject;
        enteredIndiceCache.Add((other, index)); 
        progress.hasIngredient = true;
 
        Debug.Log("Entered " + ingredient.pickup._pickupSO.name);
        CheckHasAllIngredients();
    }
    

    void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Ingredient _)) return;
        if (preventIngredientEntries) return;
        foreach (var (collider, index) in enteredIndiceCache)
        {
            if (collider != other) continue;
            ref IngredientCombinationProgression progress = ref ingredientProgress[index];
            progress.hasIngredient = false;
            progress.ingredientGO = null;
            enteredIndiceCache.Remove((collider, index));
            Debug.Log("Exited " + progress.ingredient.Pr_pickup.name);
            break;
        }

        hasAllIngredients = false;
        onNoLongerHasAllIngredients?.Invoke();
    }
    
    
    void CheckHasAllIngredients()
    {
        if (!ingredientProgress.All(p => p.hasIngredient)) return;
        Debug.Log("Cookware Ingredient Combination Loc: Has all ingredients");
        hasAllIngredients = true;
        onHasAllIngredients?.Invoke();
        TryConsumeConsumableIngredients();
    }
    
    public void SetPreventIngredientEntries(bool v) => preventIngredientEntries = v;

    public void TryConsumeConsumableIngredients()
    {
        foreach (var i in ingredientProgress)
        {
            if (!i.ingredient.ConsumableViaCookware.Contains(cookwareParent._cookwareSO)) continue;
            i.ingredientGO.gameObject.SetActive(false);
            Destroy(i.ingredientGO);
        }
    }
    
    public void ResetProgress()
    {
        for (int i = 0; i < ingredientProgress.Length; i++)
        {
            ingredientProgress[i].ingredientGO = null;
            ingredientProgress[i].hasIngredient = false;
        }
        enteredIndiceCache.Clear();
    }
}


