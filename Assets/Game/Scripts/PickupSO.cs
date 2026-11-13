using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PickupSO" , menuName = "SO/Pickup")]
public class PickupSO : ScriptableObject
{
    public string pickupName;
    public string pickupPrompt;
    public bool isIngredient;
    [Required] public GameObject Pr_pickup;
    public List<CookwareSO> ConsumableViaCookware = new();
}