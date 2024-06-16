using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class EnemyMelee : Enemy
{
    private float detectionAngle = 90;

    
    protected void Start()
    {
        if (_rsm!=null) return;
        
        IsAttacking = false;
        Health = MaxHealth;
        Animator.SetBool("IsShaking",true);
    }
    
    private void SetCanAttackTrue()
    {
        CanAttack=true;
    }
    private void SetCanAttackFalse()
    {
        CanAttack=false;
    }
    public override void StopAttack()
    {
        Animator.SetBool("IsAttacking",false);
        Animator.SetBool("AttackMelee",false);
    }

    private bool PlayerInVision()
    {
        Transform player=platform.players[IndexNearestPlayer()].transform;
        Vector3 directionToPlayer = player.position - transform.position; // Direction du joueur depuis l'ennemi
        float distanceToPlayer = directionToPlayer.magnitude; // Distance au joueur

        // Vérifier si le joueur est dans la portée du cône
        if (distanceToPlayer <= radiusAttack)
        {
            // Normaliser le vecteur de direction
            directionToPlayer.Normalize();

            // Calculer l'angle entre la direction de l'ennemi et la direction vers le joueur
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            // Vérifier si l'angle est dans les limites du cône
            if (angleToPlayer < detectionAngle / 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public async override void AttackManager()
    {
        if (!IsDead && !IsAttacking && !Animator.GetBool("Backward") && platform.players.Count != 0)
        {
            float distance = Vector3.Distance(platform.players[IndexNearestPlayer()].position, transform.position);
            if (distance <= radiusAttack)
            {
                //if (PlayerInVision())
                //{
                    FindAndLaunchAttack("Melee");
                //}
                
                FindAndLaunchAttack("Melee");
                await Task.Delay(1050);
                SoundLibrary.Instance.PlaySound("epee");
            }
            else 
            {
                StopAttack();
            }
        }
        else if (platform.players.Count==0) 
        {
            StopAttack();
        }
    }
}
