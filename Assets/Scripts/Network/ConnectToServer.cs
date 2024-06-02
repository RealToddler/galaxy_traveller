using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.OfflineMode = !GameObject.Find("GameMode").GetComponent<GameMode>().IsMultiPlayer;
        
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() 
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() 
    {
        SceneManager.LoadScene("Lobby");
    }
}