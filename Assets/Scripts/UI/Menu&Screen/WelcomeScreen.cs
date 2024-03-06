using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WelcomeScreen : Menu
{
    [SerializeField] private Text title;
    [SerializeField] private Text start;
    
    private float _alphaForTitle;
    private float _alphaForButton;
    
    private void Update()
    {
        if (_alphaForTitle < 1f)
        {
            _alphaForTitle += 0.001f;
            title.color = new Color(title.color.r, title.color.g, title.color.b, _alphaForTitle);
        }
        else
        {
            _alphaForButton += 0.01f;
            start.color = new Color(start.color.r, start.color.g, start.color.b, _alphaForButton);
        }
    }
}
