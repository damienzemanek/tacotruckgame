using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TextHolder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public UserInterface UserInterface;
    public float _timer;
    [SerializeField] public bool _textDissapears = true;
    [SerializeField] public float _dissapearTime = 4.0f;
    

    void Awake()
    {
        if(! _textDissapears)
        {
            Debug.Log("this text does not dissapear on a timer.");
            return;
        }
        else
        {
        _timer = _dissapearTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(! _textDissapears)
        {
            Debug.Log("this text does not dissapear on a timer.");
            return;
        }
        else
        {
       _timer -= Time.deltaTime;
       if (_timer <= 0)
        {
            Destroy(gameObject);
        }
        }
    }
}
