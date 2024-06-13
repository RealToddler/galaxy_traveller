using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
        else if (other.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }
}
