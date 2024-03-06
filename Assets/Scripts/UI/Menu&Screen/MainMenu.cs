using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
    public void QuitGame()
    {
        Application.Quit(0);
        Debug.Log("Quit");
    }
}
