using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [Header("Proprieties")]
    [SerializeField] public float radiusAttack;
    [SerializeField] private List<Attack> attacks;
    [SerializeField] public PlatformEnemy platform;
    [SerializeField] public float damage;  
    
    [HideInInspector] public Animator animator;
    
    //public bool IsMoving=true;
    private readonly int _maxHealth = 100;
    public float Health { get; private set; }
    public bool IsAttacking { get; set; }

    protected void Start()
    {
        animator = GetComponent<Animator>();
        
        IsAttacking = false;
        Health = _maxHealth;
    }

    private void Update()
    {
        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
        IsAttacking = animator.GetBool("IsAttacking");
    }

    protected void FindAndLaunchAttack(string attackName)
    {
        foreach (var currAttack in attacks)
        { 
            if (currAttack.Name() == attackName)
            {                
                animator.SetBool("IsAttacking",true);
                animator.SetTrigger("Attack"+attackName);
            }
        }
    }

    public int IndexNearestPlayer()
    {
        if (platform.players.Count!=0)
        {
            float distanceRes = Vector3.Distance(platform.players[0].position, transform.position);
            int res = 0;
            for(int i = 1 ; i < platform.players.Count ; i++)
            {
                float distance = Vector3.Distance(platform.players[i].position, transform.position);
                if (distance < distanceRes) 
                {
                    res = i;
                    distanceRes = distance;
                }
            }
            return res;
        }

        throw new IndexOutOfRangeException("listplayers vide");
    }
    private void StopHolding()
    // called at the beginning of IAAttackDistance animation
    {
        animator.SetBool("HoldingWeapon",false);
    }
    public void LooseHealth(float damage)
    {
        Health -= damage;
    }
}
