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
    [SerializeField] private float _rotationSpeed;
    private List<Transform> _players;
    private int _indexNearestPlayer;

    private void Start()
    {
        _players=enemy.platform.players;
        agent.stoppingDistance=enemy.radiusAttack;
    }

    void Update()
    {
        if (!enemy.IsDead)
        {
            MovementManager();
            Rotate();
            enemy.IAAnimator.SetFloat("Speed",agent.velocity.magnitude);
        }
        
    }
    
    private void MovementManager()
    {
        if ( _players.Count != 0 && !enemy.IAAnimator.GetBool("Backward"))
        {
            _indexNearestPlayer=enemy.IndexNearestPlayer();
            float distance = Vector3.Distance(_players[_indexNearestPlayer].position, enemy.transform.position);
            if (_players[_indexNearestPlayer].GetComponent<Player>().Health<=0) return ;
            if (enemy is EnemyMD)
            {
                EnemyMD newEnemy = (EnemyMD)enemy;
                if ( distance <= newEnemy.RadiusApproach && !enemy.IsAttacking && distance>newEnemy.RadiusAttackM)
                {
                    agent.isStopped=false;
                    agent.stoppingDistance=newEnemy.RadiusAttackM;
                    Approach();
                    return;
                }
                else if (distance <= newEnemy.RadiusApproach)
                {
                    agent.stoppingDistance=newEnemy.radiusAttack;
                    agent.isStopped=true;
                }
            }
            if (distance <=agent.stoppingDistance+0.1)
            {
                agent.isStopped=true;
            }
            
            else if (!enemy.IsAttacking)
            {
                agent.isStopped=false;
                Approach(); 
            }
            /*else 
            {
                enemy.IAAnimator.SetBool("IsAttacking",false);
            }*/
    
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
            Quaternion rot = Quaternion.LookRotation(_players[_indexNearestPlayer].position - transform.position);
            Vector3 direction = _players[_indexNearestPlayer].position - transform.position;
            direction.y = 0; // Annule les composantes de rotation sur les axes X et Z
            rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, _rotationSpeed * Time.deltaTime);
        }
        
    }
    void Escape()
    //called in runbackward animation
    {
        agent.isStopped=false;
        agent.SetDestination(transform.position-transform.forward*20);
    }
        
}