using System.Collections;
using System.Collections.Generic;
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

    public void Connect() {
        feedbackText.text = "";
        isConnecting = true;
        btn.interactable = false;

        if (PhotonNetwork.IsConnected) {
            LogFeedback("joining room...");
            PhotonNetwork.JoinRandomRoom();
        } else {
            LogFeedback("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.gameVersion;
        }
    }

    void LogFeedback(string message) {
        if (feedbackText == null) {
            return;
        }
        feedbackText.text += System.Environment.NewLine + message;
    }
}


