using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyAI : Ennemy
{
    [Header("Melee")]
    [SerializeField] public float meleeAttackRadius = 5f;
    [SerializeField] public float meleeStoppingDistance = 1f;

    [SerializeField] private AiMovement movement;
    
    public override void AttackManager()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (!IsAttacking)
        {
            if (distance <= meleeStoppingDistance)
            {
                Debug.Log("melee");
                IsAttacking = true;
                FindAndLaunchAttack("Melee");
            }
            else if (distance <= radiusAttackDistance)
            {
                IsAttacking = true;
                FindAndLaunchAttack("Distance");
            }
        }
    }
}
