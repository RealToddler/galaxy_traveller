// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Launcher.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in "PUN Basic tutorial" to handle typical game management requirements
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine.Serialization;

#pragma warning disable 649

/// <summary>
/// Game manager.
/// Connects and watch Photon Status, Instantiate Player
/// Deals with quiting the room and the game
/// Deals with level loading (outside the in room synchronization)
/// </summary>
public class GameManager : MonoBehaviourPunCallbacks
{

    #region Public Fields

    public static GameManager Instance;
    public Transform[] spawnPoints;
    public List<float> timeToCompleteLvl = new() {350, 350, 350};
    
    #endregion

    #region Private Fields
    
    [SerializeField] private GameObject playerPrefab;

    #endregion

    #region MonoBehaviour CallBacks

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        Instance = this;

        // in case we started this demo with the wrong scene being active, simply load the menu scene
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Menus");

            return;
        }

        if (playerPrefab == null)
        {
            // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

            Debug.LogError(
                "<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",
                this);
        }
        else
        {
            if (PhotonNetwork.InRoom && PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                
                PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoints[PhotonNetwork.PlayerList.Length-1].position, Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }
    #endregion

    #region Photon Callbacks

    public override void OnJoinedRoom()
    {
        // Note: it is possible that this MonoBehaviour is not created (or active) when OnJoinedRoom happens
        // due to that the Start() method also checks if the local player character was network instantiated!
        if (PlayerManager.LocalPlayerInstance is null)
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 20, 0), Quaternion.identity);
        }
    }

    /// <summary>
    /// Called when a Photon Player got connected. We need to then load a bigger scene.
    /// </summary>
    /// <param name="other">Other.</param>
    public void OnPlayerEnteredRoom(Player other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }
    }

    /// <summary>
    /// Called when a Photon Player got disconnected. We need to load a smaller scene.
    /// </summary>
    /// <param name="other">Other.</param>
    public void OnPlayerLeftRoom(Player other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }
    }

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menus");
    }

    #endregion

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    #endregion

    #region Private Methods

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }

        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

        PhotonNetwork.LoadLevel("PunBasics-Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    #endregion
}
