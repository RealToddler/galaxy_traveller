using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] public float radiusAttackDistance;
    [SerializeField] private Dictionary<string, Attack> attacks;
    public float Health { get; private set; }
    public bool IsAttacking { get; set;}
    
    private void Start()
    {
        Health = maxHealth;
    }

    private void Update()
    {
        if (!IsAttacking)
        {
            /*float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance <= meleeAttackRadius)
            {
                _currentStoppingDistance = meleeStoppingDistance;
                AttackMelee();
            }
            else
            {
                _currentStoppingDistance = _distanceAttackRadius;

                if (distance <= _distanceAttackRadius)
                {
                    AttackDistance();
                }
                else
                {
                    Approach();
                }

            }*/
        }
    }
}
