using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{

    [SerializeField] public Player player;

    [Header("Proprieties")]
    [SerializeField] public float radiusAttackDistance;
    [SerializeField] List<Attack> attacks;
    //[SerializeField] private PlatfromEnnemy plateform;

    public int _maxHealth = 100;

    public float Health { get; private set; }
    public bool IsAttacking { get; protected set; }

    private void Start()
    {
        IsAttacking = false;
        Health = _maxHealth;
    }

    private void Update()
    {
        AttackManager();
    }

    public virtual void AttackManager()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (!IsAttacking && distance <= radiusAttackDistance)
        {
            FindAndLaunchAttack("Distance");
        }
    }

    protected void FindAndLaunchAttack(string attackName)
    {
        foreach (var currAttack in attacks)
        {
            if (currAttack.name == attackName)
            {
                IsAttacking = true;

                currAttack.LaunchAttack();

                Invoke(nameof(BackToFalse), 2);
            }
        }
    }


    private void BackToFalse()
    {
        IsAttacking = false;
    }
}
