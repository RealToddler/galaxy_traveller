using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : BasicMenu
{
    public void BackToGame()
    {
        gameObject.SetActive(false);
    }

    public void BackToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
