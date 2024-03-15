using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    [Header("")]
    [SerializeField] private Player _player;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _attackdistanceradius;
    [SerializeField] private float _attackmeleeradius;
    [SerializeField] private int _pv = 100;
    private bool _isDead = false;
    [SerializeField] private MonoBehaviour _ennemyMovement;

    private bool _is_attacking = false;

    /*public DepIa (int pv, float attdist ,float attmel) :base(pv)
    {

      pv=50;
      _agent.speed=15;
      _attackdistanceradius=attdist;
      _attackmeleeradius=attmel;
    }
    */
    void Start()
    {
        _agent.stoppingDistance = _attackdistanceradius;
    }

    void Update()
    {
        if (!_isDead)
        {
            if (!_is_attacking)
            {
                float distance = Vector3.Distance(_player.transform.position, transform.position);
                //Debug.Log(Pv);
                if (distance <= _attackmeleeradius)
                {
                    Attackmelee();
                }
                else if (distance <= _attackdistanceradius && distance > _attackmeleeradius)
                {
                    Attackdistance();
                }
                else
                {
                    Approche();
                }
            }
        }

        //Quaternion rot =Quaternion.LookRotation(_player.transform.position-transform.position);
        //transform.rotation=Quaternion.Slerp(transform.rotation,rot,120*Time.deltaTime);
    }

    void Attackmelee()
    {
        _agent.stoppingDistance = 4;
        if (Vector3.Distance(_player.transform.position, transform.position) <= 4)
        {
            StartCoroutine(Attackplayer(10));
        }
        else
            _agent.SetDestination(_player.transform.position);
    }

    void Attackdistance()
    {
        //Debug.Log ("attack distance");
        _agent.stoppingDistance = 10;
        StartCoroutine(Attackplayer(5));
    }

    void Approche()
    {
        //Debug.Log ("approche");
        _agent.SetDestination(_player.transform.position);
        _agent.stoppingDistance = 10;
    }

    void Retirepv(int rempv)
    {
        if (_pv > 0 && !_isDead)
        {
            _pv -= rempv;
            Debug.Log(_pv);
        }
        else if (_pv <= 0 && !_isDead)
        {
            _isDead = true;
            Debug.Log("died");
        }
    }

    IEnumerator Attackplayer(int rempv)
    {
        Debug.Log("etr");
        _is_attacking = true;
        _agent.isStopped = true;
        _player.GetDamage(rempv);
        yield return new WaitForSeconds(2);

        _agent.isStopped = false;
        _is_attacking = false;
    }
}