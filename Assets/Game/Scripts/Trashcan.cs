using System;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;


public class Trashcan : MonoBehaviour
{
    private Collider _trashBox;

    void Awake()
    {
        _trashBox = GetComponent<Collider>();
        if(_trashBox == null)
        {
            throw new ArgumentException("Trashcan Collider is Null.");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Destroy(other);
    }

}