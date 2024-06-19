using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDistance : Enemy
{

    protected void Start()
    {
        IsAttacking = false;
        Health = MaxHealth;
        Animator.SetBool("HoldingWeapon",true);
    }
    void Update()
    {
        if (!IsDead)
        {
            if(Health<=0)
            {
                UpdateTriggerAnim(Animator.StringToHash("IsDead"));
                Animator.SetBool("IsAttacking",false);
                Animator.SetBool("Backward",false);
                IsDead=true;
                Invoke(nameof(SwitchScene),2);
            }
            else 
            {
                IsAttacking = Animator.GetBool("IsAttacking");
                Animator.SetBool("StopAttackDistance", !IsAttacking);
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
                Animator.SetBool("Backward",true);
            }
            else
            {
                Animator.SetBool("Backward",false);
            }

        }
    }
    private void IncreaseAttack()
    //called in iaattackdistance anim
    {
        Shots+=1;
    }
    public override void StopAttack()
    {
        Animator.SetBool("HoldingWeapon",true);
        Animator.SetBool("IsAttacking",false);
        Animator.SetBool("AttackDistance",false);
        Shots=0;
    }
    public override void AttackManager()
    {

        if (!IsDead && !IsAttacking && !Animator.GetBool("Backward") && platform.players.Count != 0)
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
    
    protected new void UpdateTriggerAnim(int anim)
    {
        photonView.RPC("TriggerAnimRPC", RpcTarget.AllBuffered, anim);  
    }
    [PunRPC]
    private void TriggerAnimRPC(int anim)
    {
        Animator.SetTrigger(anim);
    }
    
    private new void UpdateHealth(float health)
    {
        photonView.RPC(nameof(UpdateHealthRPC), RpcTarget.AllBuffered, health);  
    }
    [PunRPC]
    private void UpdateHealthRPC(float health)
    {
        print(health);
        Health = health;
    }
}
