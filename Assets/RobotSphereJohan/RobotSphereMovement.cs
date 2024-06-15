using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class RobotSphereMovement : MonoBehaviourPunCallbacks
{
    [Header("Objects")]
    [SerializeField] public GameObject explosion;

    [SerializeField] public GameObject robot;
    
    private NavMeshAgent _agent;
    private EnemyMelee _enemy;
    private Animator _animator;
    private bool _hasDestination;
    private List<Transform> _players;
    private int _indexNearestPlayer;
    private bool _playerDetected;
    

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
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    private void DetectPlayer()
    {
        _playerDetected = _enemy.platform.players.Count != 0;

        if (_playerDetected != _animator.GetBool("PlayerDetected"))
        {
            _animator.SetBool("PlayerDetected", _playerDetected);
        }
    }
    
    private void MovementManager()
    {
        if (_players.Count != 0)
        {
            _indexNearestPlayer = _enemy.IndexNearestPlayer();
            
            if(!_animator.GetBool("PlayerDetected"))
            {
                _agent.isStopped = true;
                transform.Rotate(0,1,0,Space.Self);
                _animator.SetBool("IsTurning", true);
            }
            else
            {
                _animator.SetBool("IsTurning", false);
            }
        }
        else
        {
            if (_agent.remainingDistance < 0.75f && !_hasDestination) StartCoroutine(GetNewDestination());
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().KnockBack(_enemy.damage);
            photonView.RPC("ExplodesRPC", RpcTarget.AllBuffered);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    IEnumerator GetNewDestination()
    {
        _hasDestination = true;
        yield return new WaitForSeconds(3);

        Vector3 nextDestination = transform.position;
        nextDestination += Random.Range(5, 15) * new Vector3(Random.Range(-1f, 1), 0f, Random.Range(-1f, 1f)).normalized;

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
    
    [PunRPC]
    private void ExplodesRPC()
    {
        Instantiate(explosion, transform).GetComponent<ParticleSystem>().Play();
        robot.SetActive(false);
        Invoke(nameof(Destroy), 1);
    }
}