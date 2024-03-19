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

        if (!IsAttacking && plateform.players.Count != 0)
        {
            float distance = Vector3.Distance(plateform.players[0].position, transform.position);

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
