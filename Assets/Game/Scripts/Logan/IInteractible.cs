using System;
using UnityEngine;

[Serializable]
public struct InteractionEventCtx
{
    public GameObject _gameObject;
    public string interactionPrompt;
}

public interface IInteractible
{
    InteractionEventCtx GetInteractionCtx();
}

