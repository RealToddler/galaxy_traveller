using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AiMovement : MonoBehaviour
{
    [Header("Objects")]
    
    [SerializeField] private float rotationSpeed;
    
    private List<Transform> _players;
    private int _indexNearestPlayer;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        _players = _enemy.platform.players;
        _agent.stoppingDistance = _enemy.radiusAttack;
    }

    void Update()
    {
        if (!_enemy.IsDead)
        {
            
            MovementManager();
            Rotate();
            _animator.SetFloat("Speed", _agent.velocity.magnitude);
        }
        
    }

    

    private void MovementManager()
    {
        if ( _players.Count != 0 && !_animator.GetBool("Backward"))
        {
            _indexNearestPlayer = _enemy.IndexNearestPlayer();
            float distance = Vector3.Distance(_players[_indexNearestPlayer].position, _enemy.transform.position);
            if (_players[_indexNearestPlayer].GetComponent<Player>().Health<=0) return ;
            if (_enemy is EnemyMD)
            {
                EnemyMD newEnemy = (EnemyMD)_enemy;
                if ( distance <= newEnemy.radiusApproach && !_enemy.IsAttacking && distance > newEnemy.radiusAttackM)
                {
                    _agent.isStopped = false;
                    _agent.stoppingDistance = newEnemy.radiusAttackM;
                    Approach();
                    return;
                }
                else if (distance <= newEnemy.radiusApproach)
                {
                    _agent.stoppingDistance = newEnemy.radiusAttack;
                    _agent.isStopped = true;
                }
            }
            if (distance <= _agent.stoppingDistance + 0.1)
            {
                //_agent.isStopped = true;
            }
            
            else if (!_enemy.IsAttacking)
            {
                _agent.isStopped = false;
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
        _agent.SetDestination(_players[_indexNearestPlayer].position);
        // sound walk AI
        _agent.SetDestination(_players[_indexNearestPlayer].position);
    }


    

    void Rotate()
    {
        if (_players.Count != 0)
        {
            Quaternion rot = Quaternion.LookRotation(_players[_indexNearestPlayer].position - transform.position);
            Vector3 direction = _players[_indexNearestPlayer].position - transform.position;
            direction.y = 0; // Annule les composantes de rotation sur les axes X et Z
            rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }
        
    }
    void Escape()
    //called in runbackward animation
    {
        _agent.isStopped = false;
        _agent.SetDestination(transform.position-transform.forward * 20);
    }
        
}