using System;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviourPunCallbacks
{
    public ItemData itemData;

    public void CollectItem()
    {
        photonView.RPC("SyncCollectItem", RpcTarget.AllBuffered);  
    }
    
    [PunRPC]
    public void SyncCollectItem()
    {
        Destroy(gameObject);
    }
}