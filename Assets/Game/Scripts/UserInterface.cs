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
using EMILtools.Core;

public class UserInterface : MonoBehaviour
{
    [SerializeField] public Canvas _uiCanvas;
    TextMeshProUGUI _newAddedText;
    public float _timer;

    [SerializeField] public string _textName;
    [SerializeField] public int _fontSize = 36;
    [SerializeField] public Color _textColor = Color.black;
    [SerializeField] public string _newTextReads = "Test";
    public float _xOffset = 0.0f;
    public float _yOffset= 0.0f;

    public int _page = 0;

    [SerializeField] public GameObject _uiCookbook;

    [SerializeField] public GameObject _pageLeftButton;
    [SerializeField] public GameObject _pageRightButton;

    [SerializeField] public List<GameObject> _CurrentPages = new List<GameObject>();

    bool _bookHidden = true;
    
    
    void Awake()
    {
        UpdateCookbook();
        _uiCookbook.SetActive(false);
        _pageLeftButton.SetActive(false);


    }

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.C))
        {
         ToggleCookbook();
        }
    }
    public void ToggleCookbook()
    {  
            if(! _bookHidden)
            {
            _uiCookbook.SetActive(false);
            _bookHidden = true;
            CursorEX.Set(false, CursorLockMode.Locked);
            }
            else
            {
            _uiCookbook.SetActive(true);
            _CurrentPages[_page].SetActive(false);
            _bookHidden = false;
            CursorEX.Set(true, CursorLockMode.Confined);
            _CurrentPages[_page].SetActive(true);
            }

    }

    public void PageLeft()
    {
        _page --;
        UpdateCookbook();
    }
    public void PageRight()
    {
        _page ++;
        UpdateCookbook();
    }

    public void UpdateCookbook()
    {
        _page = Mathf.Clamp(_page, 0, _CurrentPages.Count - 1);
         foreach(GameObject gameObject in _CurrentPages)
        {
            if(gameObject != _CurrentPages[_page])
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
            if(_page >= _CurrentPages.Count - 1)
            {
            _pageRightButton.SetActive(false);
            _pageLeftButton.SetActive(true);
            }
            else if(_page < 1)
            {
            _pageRightButton.SetActive(true);
            _pageLeftButton.SetActive(false);
            }
            else
            {
                _pageRightButton.SetActive(true);
                _pageLeftButton.SetActive(true);
            }
    }

    public void SpawnNewUIText(int _fontSize, Color _textColor, string _newTextReads)
    {
       GameObject _newTextObject = new GameObject(_textName);
       _newAddedText = _newTextObject.AddComponent<TextMeshProUGUI>();
       if(_uiCanvas && _newAddedText != null)
       {
        _newAddedText.alignment = TMPro.TextAlignmentOptions.Center;
        _newAddedText.fontSize = _fontSize;
        _newAddedText.autoSizeTextContainer = true;
       _newTextObject.transform.SetParent(_uiCanvas.transform);
       _newAddedText.color = _textColor;
       _newAddedText.AddComponent<TextHolder>();
        
       }
       else
        {
            Debug.LogError("HELP");

        }
        
       _newAddedText.text = _newTextReads;
       _newTextObject.transform.localPosition = new Vector2(Vector2.zero.x + _xOffset,Vector2.zero.y + _yOffset);
     
    }

}
