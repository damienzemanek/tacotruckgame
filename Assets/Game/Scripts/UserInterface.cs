using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using NUnit.Framework.Internal;
using UnityEditor;
using System.Collections.Generic;

public class UserInterface : MonoBehaviour
{
    private Canvas _uiCanvas;
    private TextMeshProUGUI _newAddedText;

    public float _timer;

    [SerializeField] public string _textName;
    [SerializeField] public int _fontSize = 36;
    [SerializeField] public float _xOffset = 0.0f;
    [SerializeField] public float _yOffset= 0.0f;
    [SerializeField] public Color _textColor = Color.black;
    [SerializeField] public bool _textDissapears = true;
    [SerializeField] public float _dissapearTime = 4.0f;
    [SerializeField] public string _newTextReads = "Test";

    
    void Awake()
    {
        _uiCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    void Update()
    {
        
    }
    public void SpawnNewUIText(float _xOffset,float _yOffset, string _textName, int _fontSize, Color _textColor,float _disappearTime,bool _textDissapears, string _newTextReads)
    {
       GameObject _newTextObject = new GameObject(_textName);
       _newAddedText = _newTextObject.AddComponent<TextMeshProUGUI>();
       if(_uiCanvas && _newAddedText != null)
       {
        _timer = _disappearTime;
        _newAddedText.alignment = TMPro.TextAlignmentOptions.Center;
        _newAddedText.fontSize = _fontSize;
        _newAddedText.autoSizeTextContainer = true;
       _newTextObject.transform.SetParent(_uiCanvas.transform);
       _newAddedText.color = _textColor;
       _newAddedText.AddComponent<TextTimer>();
        
       }
       else
        {
            Debug.Log("HELP");
            throw new ArgumentException("No Text!");
        }
        
       _newAddedText.text = _newTextReads;
       _newTextObject.transform.localPosition = new Vector2(Vector2.zero.x + _xOffset,Vector2.zero.y + _yOffset);
     
    }

}
