using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RobotSphereMovement : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] public GameObject explosion;
    
    private NavMeshAgent _agent;
    private EnemyMelee _enemy;
    private Animator _animator;
    private readonly int _animSpeed = Animator.StringToHash("Speed");
    private bool _hasDestination;
    private List<Transform> _players;
    private int _indexNearestPlayer;
    

    private void Start()
    {
        _enemy = GetComponent<EnemyMelee>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        _players = _enemy.platform.players;
    }

    void Update()
    {
        MovementManager();
        DetectPlayer();
        _animator.SetFloat("Speed",_agent.velocity.magnitude);
    }

    private void DetectPlayer()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + new Vector3(0,1,0), transform.forward, out hit, 30f))
        {
            if (hit.transform.CompareTag("Player"))
            {
                _animator.SetBool("PlayerDetected",true);
            }
        }
        
    }
    
    private void MovementManager()
    {
        
        if (_players.Count!=0)
        {
            _indexNearestPlayer = _enemy.IndexNearestPlayer();
            if(!_animator.GetBool("PlayerDetected"))
            {
                _agent.isStopped=true;
                transform.Rotate(0,1,0,Space.Self);
                _animator.SetBool("IsTurning",true);
            }
            else
            {
                _animator.SetBool("IsTurning",false);
                float distance = Vector3.Distance(_players[_indexNearestPlayer].position, _enemy.transform.position);
                if (distance<_agent.stoppingDistance+0.1)
                {
                    DestroyBotCreateExp();
                }
            }
        }
        else
        {
            if (_agent.remainingDistance<0.75f && !_hasDestination) StartCoroutine(GetNewDestination());
        }
    }

    void DestroyBotCreateExp()
    {
        gameObject.SetActive(false);
        //explosion.transform.position = transform.position;
        GameObject explo = Instantiate(explosion,transform);
        //explosion.SetActive(true);
        explo.GetComponent<ParticleSystem>().Play();
        _players[_indexNearestPlayer].gameObject.GetComponent<Player>().TakeDamage(_enemy.Damage);
        DestroyExplosion();
        Destroy(explo);

    }
    IEnumerable DestroyExplosion()
    {
        //await Task.Delay      //explosion.SetActive(false);
        yield return new WaitForSeconds(2);
        
    }

    IEnumerator GetNewDestination()
    {
        _hasDestination = true;
        yield return new WaitForSeconds(UnityEngine.Random.Range(2, 3));

        Vector3 nextDestination = transform.position;
        nextDestination += UnityEngine.Random.Range(5, 15) * new Vector3(UnityEngine.Random.Range(-1f, 1), 0f, UnityEngine.Random.Range(-1f, 1f)).normalized;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(nextDestination, out hit, 3, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
        _hasDestination = false;
    }
    void Approach()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_players[_indexNearestPlayer].position);
    }
}