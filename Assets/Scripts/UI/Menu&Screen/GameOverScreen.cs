using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menus");
    }
}
