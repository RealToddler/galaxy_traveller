using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("Player"))
        {
            obj.transform.position = respawnPoint.transform.position;
        }
    }
}
