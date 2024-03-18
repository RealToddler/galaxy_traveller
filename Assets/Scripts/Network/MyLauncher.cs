using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
public class MyLauncher : MonoBehaviourPunCallbacks
{
    public Button btn;
    public Text feedbackText;
    private byte maxPlayersPerRoom = 2;
    bool isConnecting;
    string gameVersion = "1";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        feedbackText.text = "";
        isConnecting = true;
        btn.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            LogFeedback("joining room...");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            LogFeedback("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.gameVersion;
        }
    }

    void LogFeedback(string message)
    {
        if (feedbackText == null)
        {
            return;
        }
        feedbackText.text += System.Environment.NewLine + message;
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            LogFeedback("OnconnectdToMaster: Next -> try to join Random Room");
            UnityEngine.Debug.Log("Pun Basics Tutorial/Launcher: OnConnectedToMaster() was called by Pun.");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
        UnityEngine.Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN.");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        LogFeedback("<Color=Red>OnDisconnected</Color>" + cause);
        UnityEngine.Debug.Log("PUN Bascis Tutorial/Launcher:Disconnected");
        isConnecting = false;
        btn.interactable = true;
    }

    public override void OnJoinedRoom()
    {
        LogFeedback("<Color=Green>OnJoinedRoom</Color>" + PhotonNetwork.CurrentRoom.PlayerCount);
        UnityEngine.Debug.Log("PUN Bascis Tutorial/Launcher: OnJoinedRoom() was called by PUN.");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            UnityEngine.Debug.Log("We load the 'Room for 1'");
            PhotonNetwork.LoadLevel("Lvl1");
        }
    }
}



