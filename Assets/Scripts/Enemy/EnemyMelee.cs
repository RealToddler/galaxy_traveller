using Photon.Pun;
using UnityEngine;

public class EnemyMelee : Enemy
{
    protected void Start()
    {
        if (_rsm != null) return;
        IsAttacking = false;
        Health = MaxHealth;
        Animator.SetBool("IsShaking",true);
    }
    
    private void SetCanAttackTrue()
    {
        CanAttack = true;
        AudioManager.Instance.Play("SwordAI");
    }
    private void SetCanAttackFalse()
    {
        CanAttack = false;
    }
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
    
    protected new void UpdateTriggerAnim(int anim)
    {
        photonView.RPC("TriggerAnimRPC", RpcTarget.AllBuffered, anim);  
    }
    [PunRPC]
    private void TriggerAnimRPC(int anim)
    {
        Animator.SetTrigger(anim);
    }
    
    private new void UpdateHealth(float health)
    {
        photonView.RPC(nameof(UpdateHealthRPC), RpcTarget.AllBuffered, health);  
    }
    [PunRPC]
    private void UpdateHealthRPC(float health)
    {
        print(health);
        Health = health;
    }
}
