using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PauseMenu : BasicMenu
{
    public void BackToGame()
    {
        gameObject.GetComponentInParent<PlayerUI>().ChangePauseMenuState();
    }

    public void BackToLobby()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }
}
