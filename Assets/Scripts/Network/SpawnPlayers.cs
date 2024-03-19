using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class SpawnPlayers : MonoBehaviour {
    public GameObject playerPrefab;
    public Transform spawnPoint;

    private void Start() {
        if (playerPrefab != null)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
        }
    }
}