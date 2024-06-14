using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : Attack
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _eject;
    

    

    void Start()
    {
        if (_launcher is EnemyDistance) _launcher=(EnemyDistance)_launcher;
        else if (_launcher is EnemyMD)  _launcher=(EnemyMD)_launcher;
        _name="Distance";
        _damage=_launcher.Damage;
    }

    public override void LaunchAttack()
    {
        if (_launcher.nbshots>0 && _launcher.platform.players.Count!=0 &&  _launcher.platform.players[_launcher.IndexNearestPlayer()].GetComponent<Player>().Health>0)
        {
            GameObject curr=Instantiate(_projectile, _eject.position, _eject.rotation);
            curr.GetComponent<Rigidbody>().velocity=_launcher.transform.forward*50;
            print("distance");
        }        
    }
    
}
