using UnityEngine;
using Photon.Realtime;
using Photon.Pun;


public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    void Start() 
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 16, 0), Quaternion.identity, 0);
    }

    public void OnPlayerEnterRoom(Player other) 
    {
        print(other.NickName + "s'est connecté");
    }

    public void LeaveRoom() 
    {
        PhotonNetwork.LeaveRoom();
    }

    public void QuitApplication() 
    {
        Application.Quit();
    }
}