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
        if (launcher is EnemyDistance) launcher=(EnemyDistance)launcher;
        else if (launcher is EnemyMD)  launcher=(EnemyMD)launcher;
        Name = "Distance";
        Damage = launcher.damage;
    }

    public override void LaunchAttack()
    {
        if (launcher.Shots>0 && launcher.platform.players.Count!=0 &&  launcher.platform.players[launcher.IndexNearestPlayer()].GetComponent<Player>().Health>0)
        {
            GameObject curr=Instantiate(_projectile, _eject.position, _eject.rotation);
            curr.GetComponent<Rigidbody>().velocity=launcher.transform.forward*50;
            print("distance");
            AudioManager.Instance.Play("Gun");
        }        
    }
    
}
