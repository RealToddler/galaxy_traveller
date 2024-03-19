using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void BackToMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menus");
    }
}
