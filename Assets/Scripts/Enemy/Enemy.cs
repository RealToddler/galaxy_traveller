using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [Header("Proprieties")]
    [SerializeField] public float radiusAttack;
    
    [SerializeField] List<Attack> attacks;
    [SerializeField] public PlatformEnemy platform;
    [SerializeField] public Animator IAAnimator;
    [SerializeField] public float Damage;    
    
    public bool IsDead=false;
    protected int _maxHealth = 100;
    public float Health { get; protected set; }
    public bool IsAttacking { get; set; }
    protected float EscapeRadius;
    public bool isHit=false;
    public int nbshots=0;
    protected bool _animationStarted;

    

    private void Update()
    {
        if (!IsDead)
        {
            if(Health<=0)
            {
                IAAnimator.SetTrigger("IsDead");
                IsDead=true;
            }
            else 
            {
                IsAttacking=IAAnimator.GetBool("IsAttacking");
                IAAnimator.SetBool("StopAttackMelee",!IsAttacking);
                IAAnimator.SetBool("StopAttackDistance",!IsAttacking);
                AttackManager();
            }
        }
    }

    public virtual void AttackManager(){}
    public virtual void StopAttack(){}

    protected void FindAndLaunchAttack(string attackName)
    {
        foreach (var currAttack in attacks)
        { 
            if (currAttack.Name() == attackName)
            { 
                IAAnimator.SetBool("IsAttacking",true);
                IAAnimator.SetBool("Attack"+attackName,true);
            }
        }
    }

    public int IndexNearestPlayer()
    {
        if (platform.players.Count!=0)
        {
            float distanceres=Vector3.Distance(platform.players[0].position, transform.position);
            int res=0;
            for(int i=1; i<platform.players.Count;i++)
            {
                float distance = Vector3.Distance(platform.players[i].position, transform.position);
                if (distance<distanceres) 
                {
                    res=i;
                    distanceres=distance;
                }
            }
            return res;
        }
        else throw new IndexOutOfRangeException("listplayers vide");
    }
    private void StopHolding()
    // called at the beginning of IAAttackDistance animation
    {
        IAAnimator.SetBool("HoldingWeapon",false);
        //_animationStarted=true;
    }
    public void LooseHealth(float damage)
    {
        isHit=true;
        Health -= damage;
    }
    protected virtual void FinishAnim()
    {
        if (platform.players.Count!=0)
        {
            float distanceres=Vector3.Distance(platform.players[0].position, transform.position);
            if (distanceres>radiusAttack)
            {
                StopAttack();
            }

        }

    }
    protected void Disappear()
    {
        gameObject.SetActive(false);
    }
    protected void ResetKnockback()
    {
        isHit=false;
    }
}
