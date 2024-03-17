using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100;
    public float Health { get; private set; }
    
    private void Start()
    {
        Health = maxHealth;
    }
}
