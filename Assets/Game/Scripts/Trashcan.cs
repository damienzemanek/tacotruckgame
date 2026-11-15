using System;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;


public class Trashcan : MonoBehaviour
{
    public Collider _trashBox;

    void Awake()
    {
        _trashBox = GetComponent<Collider>();
    }
    void OnTriggerEnter(Collider other)
    {
        Destroy(other);
    }

}