using UnityEngine;

[CreateAssetMenu(fileName = "PickupSO" , menuName = "SO/Pickup")]
public class PickupSO : ScriptableObject
{
    public string pickupName;
    public string pickupPrompt;
}