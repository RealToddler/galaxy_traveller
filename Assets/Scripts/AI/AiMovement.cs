using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] private Enemy enemy;
    [SerializeField] private float rotationSpeed;
    
    private List<Transform> _players;
    private int _indexNearestPlayer;

    private void Start()
    {
        _players = enemy.platform.players;
        agent.stoppingDistance = enemy.radiusAttack;
    }

    void Update()
    {
        MovementManager();
        Rotate();
        
        enemy.animator.SetFloat("Speed", agent.velocity.magnitude);
    }
    
    private void MovementManager()
    {
        if ( _players.Count != 0 && !enemy.animator.GetBool("Backward"))
        {
            _indexNearestPlayer = enemy.IndexNearestPlayer();
            float distance = Vector3.Distance(_players[_indexNearestPlayer].position, enemy.transform.position);
            if (_players[_indexNearestPlayer].GetComponent<Player>().Health<=0) return ;
            
            if (distance <= agent.stoppingDistance+0.1)
            {
                agent.isStopped = true;
            }
            else if (!enemy.IsAttacking)
            {
                agent.isStopped = false;
                Approach(); 
            }
            else 
            {
                enemy.animator.SetBool("IsAttacking",false);
            }
    
        }
    }

    void Approach()
    {
        agent.SetDestination(_players[_indexNearestPlayer].position);
    }
    void Rotate()
    {
        if (_players.Count!=0)
        {
            Vector3 direction = _players[_indexNearestPlayer].position - transform.position;
            direction.y = 0; // Annule les composantes de rotation sur les axes X et Z
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }
        
    }
    
    // called in runBackward animation
    void Escape()
    {
        agent.isStopped=false;
        agent.SetDestination(transform.position-transform.forward*20);
    }
        
}