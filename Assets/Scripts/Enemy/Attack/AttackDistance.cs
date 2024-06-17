using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AttackDistance : Attack
{
    // Start is called before the first frame update
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform eject;
    
    void Start()
    {
        if (launcher is EnemyDistance) launcher=(EnemyDistance)launcher;
        else if (launcher is EnemyMD)  launcher=(EnemyMD)launcher;
        Name = "Distance";
        Damage = launcher.damage;
    }

    public override void LaunchAttack()
    {
        if (launcher.Shots > 0 && launcher.platform.players.Count != 0 &&  launcher.platform.players[launcher.IndexNearestPlayer()].GetComponent<Player>().Health>0)
        {
            GameObject curr = Instantiate(projectile, eject.position, eject.rotation);
            curr.GetComponent<Rigidbody>().velocity = launcher.transform.forward * 50;
        }        
    }
    
}
