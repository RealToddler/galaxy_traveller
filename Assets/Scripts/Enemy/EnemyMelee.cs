using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : Enemy
{
    protected void Start()
    {
        IsAttacking = false;
        Health = _maxHealth;
        IAAnimator.SetBool("IsShaking",true);
    }
    /*void Update()
    {
        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
        IsAttacking=IAAnimator.GetBool("IsAttacking");
        AttackManager();
    }*/
    

    public override void StopAttack()
    {
        IAAnimator.SetBool("IsAttacking",false);
        IAAnimator.SetBool("AttackMelee",false);
    }
    public override void AttackManager()
    {
        if (!IsDead && !IsAttacking && !IAAnimator.GetBool("Backward") && platform.players.Count != 0)
        {
            float distance = Vector3.Distance(platform.players[IndexNearestPlayer()].position, transform.position);
            if (distance <= radiusAttack)
            {
                FindAndLaunchAttack("Melee");
            }
            else 
            {
                StopAttack();
            }
        }
        else if (platform.players.Count==0) 
        {
            StopAttack();
        }
    }
}
