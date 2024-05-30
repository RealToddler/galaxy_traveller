using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class PlatformEnemy : MonoBehaviour
{
    public List<Transform> players;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            players.Add(other.gameObject.transform);
        }    
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            players.Remove(other.gameObject.transform);
        }
    }
}
