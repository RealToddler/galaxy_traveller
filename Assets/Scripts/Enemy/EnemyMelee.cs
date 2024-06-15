using UnityEngine;

public class EnemyMelee : Enemy
{
    protected void Start()
    {
        IsAttacking = false;
        Health = MaxHealth;
        Animator.SetBool("IsShaking",true);
    }
    /*void Update()
    {
        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
        IsAttacking=IAAnimator.GetBool("IsAttacking");
        AttackManager();
    }*/
    

    public override void StopAttack()
    {
        Animator.SetBool("IsAttacking",false);
        Animator.SetBool("AttackMelee",false);
    }
    public override void AttackManager()
    {
        if (!IsDead && !IsAttacking && !Animator.GetBool("Backward") && platform.players.Count != 0)
        {
            float distance = Vector3.Distance(platform.players[IndexNearestPlayer()].position, transform.position);
            if (distance <= radiusAttack)
            {
                FindAndLaunchAttack("Melee");
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
