using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlatformEnemy : MonoBehaviour
{
    public List<Transform> players;
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            if (!players.Contains(obj.gameObject.transform))players.Add(obj.gameObject.transform);
        }
    }
    private void OnTriggerExit (Collider obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(JumpDelay),2.5f);
        }
    }
    
    void Update()
    {
        for(int i = 0; i < players.Count;i++)
        {
            if (players[i].GetComponent<Player>().IsRespawning) 
            {
                players.RemoveAt(i);
            }
        }
    }
    private void JumpDelay()
    {
        for(int i = 0; i < players.Count;i++)
        {
            if (!players[i].GetComponent<CapsuleCollider>().bounds.Intersects(this.GetComponent<MeshCollider>().bounds))
            {
                players.RemoveAt(i);
            }
        }
    }
}