using System;
using System.Collections.Generic;
using DesignPatterns.CreationalPatterns;
using EMILtools.Extensions;
using UnityEditor;
using UnityEngine;
using TMPro;

public class Interactor : MonoBehaviour
{
    [SerializeField] public GameObject _camera;
    [SerializeField] public float _interactDistance = 5f;
    [SerializeField] TextMeshProUGUI _textbox;
    public LayerMask interactionMask;

    
    public bool holdingDownMouse;
    public Action<Rigidbody> OnGrab;
    public Action OnStopInteraction;
    
    
    void Update()
    {
        CheckInteraction();
        CheckStopInteraction();
    }

    void CheckInteraction()
    {
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward, Color.red);
        _textbox.gameObject.SetActive(false);
        if (!Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactDistance, interactionMask))
            return;
        if (!hit.collider.gameObject.TryGetComponent<IInteractible>(out var interactible)) return;
        var ctx = interactible.GetInteractionCtx();
        
        if(!holdingDownMouse) _textbox.gameObject.SetActive(true);
        _textbox.SetText(ctx.interactionPrompt); //All Interactables Have interaction prompts, all interactables get it from different places
        
        if (!Input.GetMouseButtonDown(0)) return;
        
        holdingDownMouse = true;
        
        switch (interactible) //All interactables have different mutations
        {
            case Pickup pickup: 
                OnGrab?.Invoke(pickup._rb);
                pickup._rb.ResetVel3D();
                break;
            case Cookware cookware:  
                
                break;
        }
    }

    void CheckStopInteraction()
    {
        if(!Input.GetMouseButtonUp(0)) return;
        holdingDownMouse = false;
        OnStopInteraction?.Invoke();
    }
        
}



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
           order.OnFinishOrder?.Invoke();
           Debug.Log(order);
           order.complete = true;
        }
        
        
        // end
    }

}

