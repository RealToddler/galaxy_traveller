using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfTheGame : MonoBehaviour
{
    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(LaunchHappyEnd),3);
        }
    }
    void LaunchHappyEnd()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("HappyEnd");
    }
}
