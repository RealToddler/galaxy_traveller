using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    public Button joinButton;
    public Button createButton;
    public Button startButton;

    private void Start()
    {
        if (PhotonNetwork.OfflineMode)
        {
            joinInput.gameObject.SetActive(false);
            joinButton.gameObject.SetActive(false);
            createInput.gameObject.SetActive(false);
            createButton.gameObject.SetActive(false);
        }
        else
        {
            startButton.gameObject.SetActive(false);
        }
    }

    public void CreateRoom()
    {
        if (createInput.text is { Length: > 3 })
        {
            PhotonNetwork.CreateRoom(createInput.text);
        }
    }

    public void JoinRoom()
    {
        if (!PhotonNetwork.JoinRoom(joinInput.text))
        {
            print("Join failed : Message d erreur à afficher");
        }
    }

    public void StartAction()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Lvl1");
    }
    
    
}