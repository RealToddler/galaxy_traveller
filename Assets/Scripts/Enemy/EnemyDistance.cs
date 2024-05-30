using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDistance : Enemy
{

    [Header("Distance")]
    [SerializeField] public float distanceDamage;
    
    public int nbshots;
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
                StopAttack();
                animator.SetBool("Backward",true);
            }
            else
            {
                animator.SetBool("Backward",false);
            }

        }
    }
    
    // called in AIAttackDistance anim
    private void IncreaseAttack()
    {
        nbshots+=1;
    }
    private void StopAttack()
    {
        animator.SetBool("HoldingWeapon",true);
        animator.SetBool("IsAttacking",false);
        nbshots=0;
    }

    private void AttackManager()
    {
        if (!IsAttacking && !animator.GetBool("Backward") && platform.players.Count != 0)
        {
            float distance = Vector3.Distance(platform.players[IndexNearestPlayer()].position, transform.position);
            if (distance <= radiusAttack)
            {
                FindAndLaunchAttack("Distance");
            }
            else 
            {
                StopAttack();
            }
        }
    }
}
