using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private EnemyAI enemy;

    private float _currentStoppingDistance;


    private void Start()
    {
        _currentStoppingDistance = enemy.radiusAttackDistance; 
    }

    void Update()
    {
        MovementManager();
    }
    
    private void MovementManager()
    {
        if (!enemy.IsAttacking && enemy.platform.players.Count != 0)
        {
            //agent.isStopped = ennemy.IsAttacking;

            float distance = Vector3.Distance(enemy.platform.players[0].position, transform.position);

            if (distance <= enemy.meleeAttackRadius)
            {
                _currentStoppingDistance = enemy.meleeStoppingDistance;
            }
            else
            {
                _currentStoppingDistance = enemy.radiusAttackDistance;

                if (distance > enemy.radiusAttackDistance)
                {
                    Approach();
                }
            }
        }
    }

    void Approach()
    {
        agent.SetDestination(enemy.platform.players[0].position);
    }
}