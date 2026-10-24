using UnityEditor;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] public GameObject _camera;
    [SerializeField] public float _interactDistance = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetMouseButtonDown(0))
        if(Input.GetKeyDown(KeyCode.Space))
        {
        Debug.Log("Click!");

        if(Physics.Raycast(_camera.transform.position , _camera.transform.forward , out RaycastHit hit, _interactDistance))
         {
             if(hit.collider.gameObject.TryGetComponent<IInteractible>(out var interactible))
             {
                Debug.Log(hit.collider.gameObject.name);
                

                interactible.Interact();
             }
             else
                {
                    Debug.Log("NO!");
                }
            }
        }
    }
        
}