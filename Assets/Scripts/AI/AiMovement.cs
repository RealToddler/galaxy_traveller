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
        agent.isStopped = ennemy.IsAttacking;
        
        if (!ennemy.IsAttacking)
        {
            float distance = Vector3.Distance(ennemy.player.transform.position, transform.position);

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
        agent.SetDestination(ennemy.player.transform.position);
    }
}