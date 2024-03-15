using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartScreen : BasicMenu
{

    [SerializeField] private BasicMenu welcomeScreen;
    void Start()
    {
        Invoke(nameof(ToWelcomeScreen), 7.7f);
    }
    
    private void ToWelcomeScreen()
    {
        gameObject.SetActive(false);
        welcomeScreen.gameObject.SetActive(true);
    }
}
