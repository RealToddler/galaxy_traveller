using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartScreen : Menu
{

    [SerializeField] private Menu welcomeScreen;
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
