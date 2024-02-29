using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitApplication();
        }
    }

    // TODO :
    // 1. verifier que le joueur est connecte & vérifier que playerPrefab != null
    void Start() {
        PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 1, 0), Quaternion.identity, 0);
    }

    public void OnPlayerEnterRoom(Player other) {
        print(other.NickName + "s'est connecté");
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MyLauncherScene");
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    public void QuitApplication() {
        Application.Quit();
    }

}
