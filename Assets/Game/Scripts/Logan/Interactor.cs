using System;
using EMILtools.Extensions;
using UnityEditor;
using UnityEngine;
using TMPro;

public class Interactor : MonoBehaviour
{
    [SerializeField] public GameObject _camera;
    [SerializeField] public float _interactDistance = 5f;
    [SerializeField] TextMeshProUGUI _textbox;

    public Action<Rigidbody> OnGrab;
    public Action OnStopInteraction;

    void Update()
    {
        CheckInteraction();
        CheckStopInteraction();
    }

    void CheckInteraction()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        Debug.Log("Click!");

        if (!Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactDistance)) return;
        
        if(hit.collider.gameObject.TryGetComponent<IInteractible>(out var interactible))
        {
            Debug.Log(hit.collider.gameObject.name);
            var ctx =interactible.Interact();
            _textbox.gameObject.SetActive(true);
            _textbox.SetText("E to interact with: " );
            
            if (ctx._itemSOData._objectType == ItemSO.ObjectType.Pickup)
                OnGrab?.Invoke(ctx._rb);
        }
        else
            Debug.Log("NO!");
    }

    void CheckStopInteraction()
    {
        if(!Input.GetMouseButtonUp(0)) return;
        OnStopInteraction?.Invoke();
    }
        
}