using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [Header("Proprieties")]
    [SerializeField] public float radiusAttackDistance;
    [SerializeField] List<Attack> attacks;
    [SerializeField] public PlatformEnemy platform;

    public int maxHealth = 100;

    public float Health { get; private set; }
    public bool IsAttacking { get; protected set; }

    private void Start()
    {
        IsAttacking = false;
        Health = maxHealth;
    }

    private void Update()
    {
        AttackManager();

        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void AttackManager()
    {
        if (platform.players.Count != 0)
        {
            float distance = Vector3.Distance(platform.players[0].position, transform.position);

            if (!IsAttacking && distance <= radiusAttackDistance)
            {
                FindAndLaunchAttack("Distance");
            }
        }
    }

    protected void FindAndLaunchAttack(string attackName)
    {
        foreach (var currAttack in attacks)
        {
            if (currAttack.name == attackName)
            {
                IsAttacking = true;
                
                //currAttack.LaunchAttack();
                
                

                Invoke(nameof(BackToFalse), 2);
            }
        }
    }


    private void BackToFalse()
    {
        IsAttacking = false;
    }

    public void LooseHealth(float damage)
    {
        Health -= damage;
    }
}
