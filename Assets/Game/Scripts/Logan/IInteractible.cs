using System;
using UnityEngine;

public interface IInteractible
{
    [Serializable]
    public struct ItemContext
    {
        public GameObject _gameObject;
        public Rigidbody _rb;
        public ItemSO _itemSOData;
    }
    ItemContext Interact();
} 
