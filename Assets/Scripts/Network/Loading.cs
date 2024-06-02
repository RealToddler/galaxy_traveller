using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.OfflineMode = !GameObject.Find("GameMode").GetComponent<GameMode>().IsMultiPlayer;

        if (PhotonNetwork.OfflineMode)
        {
            print("single");
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
        else
        {
            print("Multi");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster() 
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() 
    {
        SceneManager.LoadScene("Lobby");
    }
    
    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel("Lvl1");
    }
}