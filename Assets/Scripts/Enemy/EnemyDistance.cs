using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDistance : Enemy
{

    protected void Start()
    {
        IsAttacking = false;
        Health = _maxHealth;
        IAAnimator.SetBool("HoldingWeapon",true);
    }
    void Update()
    {
        if (!IsDead)
        {
            if(Health<=0)
            {
                IAAnimator.SetTrigger("IsDead");
                IAAnimator.SetBool("IsAttacking",false);
                IsDead=true;
            }
            else 
            {
                IsAttacking=IAAnimator.GetBool("IsAttacking");
                IAAnimator.SetBool("StopAttackDistance", !IsAttacking);
                AttackManager();
                CheckForEscape();
            }
        }
        
    }
    private void CheckForEscape()
    {
        if (platform.players.Count != 0)
        {
            float distance = Vector3.Distance(platform.players[IndexNearestPlayer()].position, transform.position);
            if (distance<=2.5)
            {
                StopAttack();
                IAAnimator.SetBool("Backward",true);
            }
            else
            {
                IAAnimator.SetBool("Backward",false);
            }

        }
    }
    private void IncreaseAttack()
    //called in iaattackdistance anim
    {
        nbshots+=1;
    }
    public override void StopAttack()
    {
        IAAnimator.SetBool("HoldingWeapon",true);
        IAAnimator.SetBool("IsAttacking",false);
        IAAnimator.SetBool("AttackDistance",false);
        nbshots=0;
    }
    public override void AttackManager()
    {

        if (!IsDead && !IsAttacking && !IAAnimator.GetBool("Backward") && platform.players.Count != 0)
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
        else if (platform.players.Count==0)StopAttack();
    }
    
}
