using Photon.Pun;
using UnityEngine;

public class ItemSync : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 networkedPosition;
    private Quaternion networkedRotation;
    
    void Start()
    {
        if (photonView.IsMine)
        {
            // This object is controlled by the local player
        }
        else
        {
            // This object is controlled by the remote player
        }
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, networkedPosition, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkedRotation, Time.deltaTime * 10);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Local player sends data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Remote player receives data
            networkedPosition = (Vector3)stream.ReceiveNext();
            networkedRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}