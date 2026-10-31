using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static IInteractible;


public class Cookware : MonoBehaviour, IInteractible
{
    [SerializeField] CookwareSO _cookwareSO;
    public InteractionEventCtx GetInteractionCtx() => new()
    {
        _gameObject = gameObject,
        interactionPrompt = _cookwareSO.cookwarePrompt,
    };
}