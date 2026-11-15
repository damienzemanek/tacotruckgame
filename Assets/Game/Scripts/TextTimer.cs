using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TextTimer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public UserInterface UserInterface;
    public float _timer;

    void Awake()
    {
        UserInterface = GameObject.Find("Canvas").GetComponentInChildren<UserInterface>();
        _timer = UserInterface._timer;
    }
    // Update is called once per frame
    void Update()
    {
       _timer -= Time.deltaTime;
       if (_timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
