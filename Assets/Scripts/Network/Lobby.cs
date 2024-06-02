using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    
    private void Update()
    {
        if (!Cursor.visible)
        {
            Cursor.visible = true;
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

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Lvl1");
    }
}