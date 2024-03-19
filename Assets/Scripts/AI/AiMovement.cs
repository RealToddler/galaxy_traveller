using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private EnnemyAI ennemy;

    private float _currentStoppingDistance;


    private void Start()
    {
        _currentStoppingDistance = ennemy.radiusAttackDistance; 
    }

    void Update()
    {
        MovementManager();
    }
    
    private void MovementManager()
    {
        if (!ennemy.IsAttacking && ennemy.plateform.players.Count != 0)
        {
            agent.isStopped = ennemy.IsAttacking;

            float distance = Vector3.Distance(ennemy.plateform.players[0].position, transform.position);

            if (distance <= ennemy.meleeAttackRadius)
            {
                _currentStoppingDistance = ennemy.meleeStoppingDistance;
            }
            else
            {
                _currentStoppingDistance = ennemy.radiusAttackDistance;

                if (distance > ennemy.radiusAttackDistance)
                {
                    Approach();
                }
            }
        }
    }

    void Approach()
    {
        agent.SetDestination(ennemy.plateform.players[0].position);
    }
}