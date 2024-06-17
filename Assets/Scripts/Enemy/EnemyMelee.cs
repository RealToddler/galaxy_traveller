using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class EnemyMelee : Enemy
{
    protected void Start()
    {
        if (_rsm!=null) return;
        IsAttacking = false;
        Health = MaxHealth;
        Animator.SetBool("IsShaking",true);
    }
    
    private void SetCanAttackTrue()
    {
        CanAttack=true;
        AudioManager.Instance.Play("SwordAI");
    }
    private void SetCanAttackFalse()
    {
        CanAttack=false;
    }
    public override void StopAttack()
    {
        Animator.SetBool("IsAttacking",false);
        Animator.SetBool("AttackMelee",false);
    }
    public override void AttackManager()
    {
        if (!IsDead && !IsAttacking && !Animator.GetBool("Backward") && platform.players.Count != 0)
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
