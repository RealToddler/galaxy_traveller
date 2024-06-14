using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

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
            //print (obj.GetComponent<Rigidbody>().velocity.y);
            //if (obj.GetComponent<Rigidbody>().velocity.y<0.1) players.Remove(obj.gameObject.transform);
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