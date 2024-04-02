using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlane : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("Player"))
        {
            obj.GetComponent<Player>().transform.position = spawnPoint.position;
            obj.GetComponent<Player>().TakeDamage(25);
        }
    }
}
