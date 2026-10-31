using System;
using System.Collections.Generic;
using DesignPatterns.CreationalPatterns;
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

    public List<Order> orders;

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
        
        
        // end
    }

}