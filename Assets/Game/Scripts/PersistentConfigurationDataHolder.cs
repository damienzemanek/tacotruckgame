using DesignPatterns.CreationalPatterns;
using UnityEngine;

public class PersistentConfigurationDataHolder : Singleton<PersistentConfigurationDataHolder>
{
    public IngredientCombinationDb ingredientCombinationDb;
}
