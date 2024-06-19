using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAfterEnnemyDeath : MonoBehaviour
{
    [SerializeField] public Enemy enemy;
    [SerializeField] private GameObject platform;

    private void Update()
    {
        if (enemy.IsDead)
        {
            platform.SetActive(true);
        }
    }
}
