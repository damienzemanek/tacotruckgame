using UnityEditor;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] public GameObject _camera;
    [SerializeField] public float _interactDistance = 5f;

    void Update()
    {
        CheckInteraction();
    }

    void CheckInteraction()
    {
        //if (Input.GetMouseButtonDown(0))
        if (!Input.GetMouseButtonDown(0)) return;
        
        Debug.Log("Click!");

        if (!Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactDistance)) return;
        
        if(hit.collider.gameObject.TryGetComponent<IInteractible>(out var interactible))
        {
            Debug.Log(hit.collider.gameObject.name);
            interactible.Interact();
        }
        else
            Debug.Log("NO!");
    }
        
}