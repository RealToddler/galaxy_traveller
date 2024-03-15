using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public int maxOxygen = 100;
    public float Health { get; private set ; }
    public float Oxygen { get; private set ; }

    private void Start()
    {
        Health = maxHealth;
        Oxygen = maxOxygen;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    
    // Remove damage to player health
    public void GetDamage(float damage)
    {
        if (Health > 0)
        {
            Health -= damage;
        }
        else
        {
            // Respawn or GameOver
        }
    }
    
    // Remove qty of O2 to player Oxygene
    public void LooseOxygen(float qty)
    {
        if (Oxygen > 0)
        {
            Oxygen -= qty;
        }
        else
        {
            // Respawn or GameOver
        }
    }
    
    // ==== Photon ====
    public string NickName { get; } // Photon oblige mais je pense emilien sait comment gerer ca
    
}
