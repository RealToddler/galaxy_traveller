using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class PlatformEnemy : MonoBehaviour
{
    public List<Transform> players;
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            players.Add(obj.gameObject.transform);
        }
    }
}
