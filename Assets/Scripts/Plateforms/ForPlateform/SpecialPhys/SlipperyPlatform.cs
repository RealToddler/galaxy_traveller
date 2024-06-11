using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyPlatform : MonoBehaviour
{
    [SerializeField] private float slipperyForce = 15f;

    private void Start()
    {
        gameObject.GetComponent<Collider>().material.dynamicFriction = 0;        
        gameObject.GetComponent<Collider>().material.staticFriction = 0;
        gameObject.GetComponent<Collider>().material.frictionCombine = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.material.dynamicFriction = 0;        
            collision.collider.material.staticFriction = 0;
            collision.collider.material.frictionCombine = 0;
            // collision.gameObject.GetComponent<MoveBehaviour>().Bounce();
        }
    }
}
