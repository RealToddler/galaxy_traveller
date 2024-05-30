using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Attack
{
    // Start is called before the first frame update
    void Start()
    {
        _name="Melee";
        _damage=_launcher.damage;
    }
    public override void LaunchAttack()
    {
        List<Transform> _players=_launcher.platform.players;
        int _indexNearestPlayer=_launcher.IndexNearestPlayer();
        Transform player=_players[_indexNearestPlayer]; // Référence au joueur

        float fieldOfVisionAngle = 180f; // Angle de vision de l'ennemi
        Vector3 directionToPlayer = player.position - transform.position;// Vecteur du joueur à l'ennemi
        Vector3 forwardDirection = transform.forward;// Vecteur avant de l'ennemi
        float angle = Vector3.Angle(directionToPlayer, forwardDirection);

        if (angle < fieldOfVisionAngle * 0.5f)// Vérifie si le joueur est dans le champ de vision de l'ennemi
        {
            RaycastHit hit;
            
            if (Physics.Raycast(_launcher.gameObject.GetComponent<Transform>().position + new Vector3(0,1,0),directionToPlayer, out hit, 2f))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    hit.collider.GetComponent<Player>().TakeDamage(_damage);
                }
            }
        }
    }
}
