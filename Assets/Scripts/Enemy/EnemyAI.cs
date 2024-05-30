using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : Enemy
{

    [Header("Melee")]
    [SerializeField] public float meleeAttackRadius = 2f;
    [SerializeField] public float meleeStoppingDistance=2f;
    public int nbShots;
    
    void Update()
    {
        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }

        IsAttacking = animator.GetBool("IsAttacking");
        
        AttackManager();
        CheckForEscape();
    }
    private void CheckForEscape()
    {
        if (platform.players.Count != 0)
        {
            float distance = Vector3.Distance(platform.players[IndexNearestPlayer()].position, transform.position);
            if (distance<=2.5)
            {
                animator.SetBool("IsAttacking",false);
                animator.SetBool("HoldingWeapon",true);
                animator.SetBool("Backward",true);
                nbShots = 0;
            }
            else
            {
                animator.SetBool("Backward",false);
            }

        }
    }
    private void IncreaseAttack()
    {
        nbShots += 1;
    }

    private void AttackManager()
    {
        if (!IsAttacking && !animator.GetBool("Backward") && platform.players.Count != 0)
        {
            float distance = Vector3.Distance(platform.players[IndexNearestPlayer()].position, transform.position);
            if (distance <= meleeStoppingDistance)
            {
                //Debug.Log("melee");
                FindAndLaunchAttack("Melee");
                
            }
            else if (distance <= radiusAttack)
            {
                //Debug.Log("Distance");
                /*if (!IsMoving)*/FindAndLaunchAttack("Distance");
                //else IAAnimator.SetBool("HoldingWeapon",true);
            }
            else 
            {
                animator.SetBool("HoldingWeapon",true);
                animator.SetBool("IsAttacking",false);
                //IsAttacking=false;
                nbShots = 0;
            }
        }
    }
}
