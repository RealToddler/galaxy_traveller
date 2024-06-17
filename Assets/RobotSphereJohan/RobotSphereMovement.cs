using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class RobotSphereMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] public GameObject explosion;
    [SerializeField] public GameObject robot;

    private NavMeshAgent _agent;
    private EnemyMelee _enemy;
    private Animator _animator;
    private bool _hasDestination;
    private List<Transform> _players;
    private int _indexNearestPlayer;
    private bool _playerDetected;
    private Vector3 _spawnPoint;


    private void Start()
    {
        _enemy = GetComponent<EnemyMelee>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _players = _enemy.platform.players;
        _spawnPoint = transform.position;

        if (!PhotonNetwork.IsMasterClient)
        {
            GetComponent<NavMeshAgent>().enabled = false;
        }
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
        MovementManager();
        DetectPlayer();

        FloatAnim("Speed", _agent.velocity.magnitude);
    }

    private void DetectPlayer()
    {
        if (_players.Count == 0)
        {
            LaunchBoolAnim("PlayerDetected", false);
            _agent.speed = 5;
        }
        else
        {
            _agent.speed = 8;
            RaycastHit hit;

            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, out hit, 30f))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    LaunchBoolAnim("PlayerDetected", true);
                    _agent.isStopped = false;
                }
            }
        }
    }

    private void MovementManager()
    {
        if (_players.Count != 0)
        {
            _indexNearestPlayer = _enemy.IndexNearestPlayer();

            if (!_animator.GetBool("PlayerDetected"))
            {
                _agent.isStopped = true;
                transform.Rotate(0, 2, 0, Space.Self);
                LaunchBoolAnim("IsTurning", true);
            }
            else
            {
                LaunchBoolAnim("IsTurning", false);
                _agent.SetDestination(_players[_indexNearestPlayer].position);
            }
        }
        else
        {
            _agent.SetDestination(_spawnPoint);
            _agent.isStopped = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().KnockBack(_enemy.damage);
            AudioManager.Instance.Play("Explosion");
            photonView.RPC("ExplodesRPC", RpcTarget.AllBuffered);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    [PunRPC]
    private void ExplodesRPC()
    {
        Instantiate(explosion, transform).GetComponent<ParticleSystem>().Play();
        robot.SetActive(false);
        Invoke(nameof(Destroy), 0.5f);
    }
    
    // ======================================= Animation RPC ============================================
    private void LaunchBoolAnim(string anim, bool value)
    {
        photonView.RPC("LaunchBoolAnimRPC", RpcTarget.AllBuffered, anim, value);
    }
    
    [PunRPC]
    private void LaunchBoolAnimRPC(string anim, bool value)
    {
        _animator.SetBool(anim, value);
    }
    
    private void FloatAnim(string anim, float value)
    {
        photonView.RPC("FloatAnimRPC", RpcTarget.AllBuffered, anim, value);
    }
    
    [PunRPC]
    private void FloatAnimRPC(string anim, float value)
    {
        _animator.SetFloat(anim, value);
    }
}