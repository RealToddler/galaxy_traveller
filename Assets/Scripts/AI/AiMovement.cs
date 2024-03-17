using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Player player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Ennemy ennemy;
    
    [Header("Melee")]
    [SerializeField] private float meleeAttackRadius = 5f;
    [SerializeField] private float meleeStoppingDistance = 1f;

    private float _currentStoppingDistance;
    private float _distanceAttackRadius;


    private void Start()
    {
        _distanceAttackRadius = ennemy.radiusAttackDistance;
    }

    void Update()
    {
        agent.stoppingDistance = _currentStoppingDistance;
        
        if (!ennemy.IsAttacking)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            
            if (distance <= meleeAttackRadius)
            {
                _currentStoppingDistance = meleeStoppingDistance;
                AttackMelee();
            }
            else
            {
                _currentStoppingDistance = _distanceAttackRadius;
                
                if (distance <= _distanceAttackRadius)
                {
                    AttackDistance();
                }
                else
                {
                    Approach();
                }

            }
        }
    }

    void AttackMelee()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 2f)
        {
            Debug.Log("Attaque melee");
            StartCoroutine(AttackPlayer(10));
        }
    }

    void AttackDistance()
    {
        Debug.Log("Attaque a distance");
        StartCoroutine(AttackPlayer(5));
    }

    void Approach()
    {
        agent.SetDestination(player.transform.position);
    }

    IEnumerator AttackPlayer(int damage)
    {
        ennemy.IsAttacking = true;
        agent.isStopped = true;
        
        player.GetDamage(damage);
        
        yield return new WaitForSeconds(2);
        
        agent.isStopped = false;
        ennemy.IsAttacking = false;
    }
}