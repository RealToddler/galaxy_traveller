using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviourPunCallbacks
{
    [Header("Attack's proprieties")]
    [SerializeField] public float radiusAttack;
    [SerializeField] List<Attack> attacks;
    [SerializeField] public float damage;
    
    [Header("Other")]
    [SerializeField] public PlatformEnemy platform;
    [SerializeField] private int nextLvl;
    
    public bool IsDead { get; protected set; }
    protected Animator Animator;
    protected const int MaxHealth = 100;
    public float Health { get; protected set; }
    public bool IsAttacking { get; protected set; }
    protected float EscapeRadius;
    public bool IsHit { get; private set; }
    public int Shots { get; protected set; }
    [HideInInspector]
    public bool CanAttack;
    protected bool AnimationStarted;
    protected RobotSphereMovement _rsm;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _rsm=GetComponent<RobotSphereMovement>();
    }

    private void Update()
    {
        if (_rsm != null) return;
        
        if (!IsDead)
        {
            IsAttacking = Animator.GetBool("IsAttacking");
            Animator.SetBool("StopAttackMelee", !IsAttacking);
            Animator.SetBool("StopAttackDistance", !IsAttacking);
            AttackManager();
            if (Health == 0)
            {
                IsDead = true;
                Invoke(nameof(SwitchScene), 4);
            }
        }
    }

    public virtual void AttackManager(){}
    public virtual void StopAttack(){}

    protected void FindAndLaunchAttack(string attackName)
    {
        foreach (var currAttack in attacks)
        { 
            if (currAttack.Name == attackName)
            {
                Animator.SetBool("IsAttacking",true);
                Animator.SetBool("Attack" + attackName,true);
                Invoke(nameof(ResetKnockback), 0.9f);
            }
        }
    }

    public int IndexNearestPlayer()
    {
        if (platform.players.Count != 0)
        {
            float distanceres = Vector3.Distance(platform.players[0].position, transform.position);
            int res = 0;
            for(int i = 1; i < platform.players.Count; i++)
            {
                float distance = Vector3.Distance(platform.players[i].position, transform.position);
                if (distance < distanceres) 
                {
                    res = i;
                    distanceres = distance;
                }
            }
            return res;
        }

        return -1;
    }
    private void StopHolding()
    // called at the beginning of IAAttackDistance animation
    {
        Animator.SetBool("HoldingWeapon",false);
    }
    
    public void LooseHealth(float damages)
    {
        if (Health - damages > 0)
        {
            Health -= damages;
            IsHit = true;
        }
        else
        {
            Health = 0;
            IsDead = true;
            if (_rsm != null) return;
            UpdateTriggerAnim(Animator.StringToHash("IsDead"));
            IsDead = true;
            Invoke(nameof(SwitchScene), 4);
        }
        
        UpdateHealth(Health);
    }
    
    protected virtual void FinishAnim()
    {
        if (platform.players.Count != 0)
        {
            float distanceres = Vector3.Distance(platform.players[0].position, transform.position);
            if (distanceres > radiusAttack)
            {
                StopAttack();
            }
        }
    }
    protected void ResetKnockback()
    {
        IsHit = false;
    }

    protected void SwitchScene()
    {
        PlayerManager.LocalPlayerInstance.gameObject.GetComponent<Player>().NextLevel(nextLvl);
    }

    public void KnockBack()
    {
        UpdateTriggerAnim(Animator.StringToHash("Knockback"));
    }
    
    // ======================================= Animation RPC ============================================
    protected void UpdateFloatAnim(string anim, float value)
    {
        photonView.RPC("FloatAnimRPC", RpcTarget.AllBuffered, anim, value);
    }
    
    [PunRPC]
    private void FloatAnimRPC(string anim, float value)
    {
        Animator.SetFloat(anim, value);
    }
    
    protected void UpdateTriggerAnim(int anim)
    {
        photonView.RPC(nameof(TriggerAnimRPC), RpcTarget.AllBuffered, anim);  
    }
    [PunRPC]
    private void TriggerAnimRPC(int anim)
    {
        Animator.SetTrigger(anim);
    }

    protected void UpdateHealth(float health)
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
