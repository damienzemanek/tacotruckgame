using UnityEngine;

[CreateAssetMenu(menuName = "SO/Customers", fileName = "Customers")]
public class CustomersConfig : ScriptableObject
{
    public string[] customers;
    public string GetName(int index) => customers[index];
}