using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

//using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
      //public Camera cam;
      [SerializeField]
      private GameObject _player;
      [SerializeField]
      private NavMeshAgent _ag;
      [SerializeField]
      private float _attackdistanceradius;
      [SerializeField]
      private float _attackmeleeradius;
      [SerializeField]
      private int _pv=100;
      private bool is_dead=false;
      [SerializeField]
      private MonoBehaviour ennemymovement;
      private bool _is_attacking=false;
      /*public DepIa (int pv, float attdist ,float attmel) :base(pv)
      {

        pv=50;
        _ag.speed=15;
        _attackdistanceradius=attdist;
        _attackmeleeradius=attmel;
      }
      */
      void Start()
      {
        _ag.stoppingDistance=_attackdistanceradius;
      }
      void Update()
      {
        if (!is_dead)
        {
          if (!_is_attacking)
          {
            float distance=Vector3.Distance(_player.transform.position, transform.position );
            //Debug.Log(Pv);
            if (distance<=_attackmeleeradius)
            {
              Attackmelee();
            }
            else if (distance<=_attackdistanceradius && distance>_attackmeleeradius) 
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
        _ag.stoppingDistance=4;
        if (Vector3.Distance(_player.transform.position, transform.position )<=4)
        {
          
          StartCoroutine(Attackplayer(10));
        }
        else 
        _ag.SetDestination(_player.transform.position);
      }
      void Attackdistance()
      {
        //Debug.Log ("attack distance");
        _ag.stoppingDistance=10;
        StartCoroutine(Attackplayer(5));
      }
      void Approche ()
      {
        //Debug.Log ("approche");
        _ag.SetDestination(_player.transform.position);
        _ag.stoppingDistance=10;
        
      }
      void Retirepv(int rempv)
      {
        
        if (_pv>0 && !is_dead)
        {
          _pv-=rempv;
          Debug.Log(_pv);
            
        }
        else if (_pv<=0 && !is_dead)
        {
          is_dead=true;
          Debug.Log("died");
          
        }
       
        
      }
      IEnumerator Attackplayer(int rempv)
      {
        Debug.Log("etr");
        _is_attacking=true;
        _ag.isStopped=true;
        Retirepv(rempv);
        yield return new WaitForSeconds(2);
        
        _ag.isStopped=false;
        _is_attacking=false;
      }
      
}