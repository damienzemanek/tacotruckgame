using System;
using System.Collections.Generic;
using DesignPatterns.CreationalPatterns;
using DG.Tweening;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class OrderTracker : Singleton<OrderTracker>
{
    
    [Serializable]
    public class Order
    {
        public Action OnFinishOrder;
        public string name;
        public bool complete;
    }

    public float walkUpDuration = 4f;
    [Required] public IngredientPlacementZone placementZone;
    [Required] public CustomersConfig customersConfig;
    [Required] public GameObject customerPrefab;
    [Required] public Transform custSpawnLoc;
    [Required] public Transform windowWalkUpLoc;
    [ReadOnly] public int currentCustomer = 0;
    [ReadOnly] public List<Order> orders = new();

    private void Start()
    {
        StartGame();
    }

    public void FinishOrder(string orderName)
    {
        // for logan: make foreach loop that calls .Invoke() on the order
        // then mark the order as complete

        // for damien: Thank you for providing more context, I think I did what you asked within the code, but I might have followed it to stringently. Please message me if you need changed to be made.
        
        foreach(Order order in orders)
        {
            if(order.name == orderName)
            {
                order.OnFinishOrder?.Invoke();
                Debug.Log(order);
                order.complete = true;
            }
            else
            {
                return;
            }
        }
        SpawnCustomer();
        
        // end
    }

    public void StartGame()
    {
        currentCustomer = -1;
        SpawnCustomer();
    }
    [Button]
    public void SpawnCustomer()
    {
        currentCustomer++;
        Sequence seq = DOTween.Sequence();
        var newCustomer = Instantiate(customerPrefab, custSpawnLoc);
        newCustomer.transform.SetParent(null);
        newCustomer.transform.position = Vector3.zero;
        var customer = newCustomer.Get<Customer>();
        customer.config = customersConfig;
        customer.myIndex = currentCustomer;
        seq.Append(newCustomer.transform.DOLocalMove(windowWalkUpLoc.transform.position, walkUpDuration));
        seq.AppendCallback(customer.AvaliableToTakeOrder);
        seq.Play();
    }


    public void SubscribeCustomer(string customerName, Action onFinishOrder)
    {
        Order newOrder = new()
        {
            name = customerName,
            OnFinishOrder = onFinishOrder,
            complete = false
        };
        orders.Add(newOrder);
    }

}