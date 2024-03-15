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
    
    [Header("Attack Radius")]
    [SerializeField] private float meleeAttackRadius = 5f;
    [SerializeField] private float distanceAttackRadius = 10f;
    
    [Header("Others")]
    [SerializeField] private float meleeDistance = 1f;

    private float _currentStoppingDistance;
    private bool _isAttacking;

    void Update()
    {
        agent.stoppingDistance = _currentStoppingDistance;
        
        if (!_isAttacking)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            
            if (distance <= meleeAttackRadius)
            {
                _currentStoppingDistance = meleeDistance;
                AttackMelee();
            }
            else
            {
                _currentStoppingDistance = distanceAttackRadius;
                
                if (distance <= distanceAttackRadius)
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
        _isAttacking = true;
        agent.isStopped = true;
        
        player.GetDamage(damage);
        
        yield return new WaitForSeconds(2);
        
        agent.isStopped = false;
        _isAttacking = false;
    }
}