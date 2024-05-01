using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System;
using Photon.Realtime;


public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    static public SpawnPlayers Instance;
    public Transform spawnPoint;
    // public PhotonView view;


    private void Start() {
        Instance = this;
        if (playerPrefab != null)
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
            }
        }
    }
}