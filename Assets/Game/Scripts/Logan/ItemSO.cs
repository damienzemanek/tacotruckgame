using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO" , menuName = "SO/ItemSO")]
public class ItemSO : ScriptableObject
{
    public enum ObjectType
    {
    Cookware,
    Pickup   
    }
    public ObjectType _objectType;
}

