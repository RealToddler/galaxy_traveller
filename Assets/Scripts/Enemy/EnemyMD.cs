using UnityEngine;

public class EnemyMD : Enemy
{
    [Header("Melee")]
    [SerializeField] public float radiusAttackM;
    [SerializeField] public float radiusApproach;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject gun;
    
    private float _distance;
    
    private static readonly int StoppingAttackMelee = Animator.StringToHash("StopAttackMelee");
    private static readonly int StoppingAttackDistance = Animator.StringToHash("StopAttackDistance");
    private static readonly int IsShaking = Animator.StringToHash("IsShaking");
    private static readonly int HoldingWeapon = Animator.StringToHash("HoldingWeapon");
    private static readonly int Attacking = Animator.StringToHash("IsAttacking");
    private static readonly int IsDeadAnim = Animator.StringToHash("IsDead");
    private static readonly int AttackDistance = Animator.StringToHash("AttackDistance");
    private static readonly int AttackMelee = Animator.StringToHash("AttackMelee");
    private static readonly int Backward = Animator.StringToHash("Backward");


    void Start()
    {
        IsAttacking = false;
        Health = MaxHealth;
        _distance = radiusAttack + 1;
    }
    void Update()
    {
        if (!base.IsDead)
        {
            if(Health <= 0)
            {
                Animator.SetTrigger(IsDeadAnim);
                Animator.SetBool(Attacking, false);
                base.IsDead = true;
            }
            else 
            {
                IsAttacking = Animator.GetBool(Attacking);
                AttackManager();
                CheckForChanging();
                Animator.SetBool(StoppingAttackMelee, gun.activeSelf || IsAttacking == false);
                Animator.SetBool(StoppingAttackDistance, sword.activeSelf || IsAttacking == false);
                Animator.SetBool(IsShaking, sword.activeSelf);
                Animator.SetBool(HoldingWeapon, gun.activeSelf && !IsAnimationPlaying("AttackDistance"));
                if (platform.players.Count != 0)_distance=Vector3.Distance(platform.players[IndexNearestPlayer()].position, transform.position);
                gun.SetActive(_distance > radiusApproach);
                sword.SetActive(_distance <= radiusApproach);
            }
        }
        
    }
    private void SetCanAttackTrue()
    {
        CanAttack=true;
    }
    private void SetCanAttackFalse()
    {
        CanAttack=false;
    }
    private void CheckForChanging()
    {
        if (platform.players.Count != 0)
        {
            if (_distance<=radiusApproach && Animator.GetBool(AttackDistance))
            {
                StopAttackDistance();
            }
            else if (_distance>radiusApproach && Animator.GetBool(AttackMelee) && _distance<=radiusAttack )
            {
                StopAttackMelee();
            }
        }
    }
    bool IsAnimationPlaying(string animName)
    {
        // Récupère les informations de l'état actuel de l'Animator
        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(1);
        // Vérifie si le nom de l'état contient le nom de l'animation
        return stateInfo.IsName(animName);
    }
    private void IncreaseAttack()
    //called in iaattack_distance anim
    {
        Shots+=1;
    }
    public void StopAttackMelee()
    {
        Animator.SetBool(HoldingWeapon, true);
        Animator.SetBool(Attacking, false);
        Animator.SetBool(AttackMelee, false);
        Shots = 0;
    }
    public void StopAttackDistance()
    {
        Animator.SetBool(HoldingWeapon, false);
        Animator.SetBool(Attacking, false);
        Animator.SetBool(AttackDistance, false);
        Shots = 0;
    }
    public override void StopAttack()
    {
        Animator.SetBool(Attacking, false);
        Animator.SetBool(AttackDistance, false);
        Animator.SetBool(AttackMelee, false);
        Animator.SetBool(StoppingAttackMelee, true);
        Animator.SetBool(StoppingAttackDistance, true);
        Shots = 0;
    }
    protected override void FinishAnim()
    {
        if (platform.players.Count != 0)
        {
            if (_distance > radiusAttack )
            {
                StopAttack();
                Animator.SetBool(HoldingWeapon,true);
            }
            else if ( _distance <= radiusApproach && _distance > radiusAttackM)
            {
                StopAttack();
            }

        }
    }
    public override void AttackManager()
    {
        if (!base.IsDead && !IsAttacking && !Animator.GetBool(Backward) && platform.players.Count != 0)
        {
            if (_distance <= radiusAttackM)
            {
                FindAndLaunchAttack("Melee");
            }
            else if (_distance<=radiusApproach)
            {

            }
            else if (_distance<=radiusAttack)
            {
                FindAndLaunchAttack("Distance");
            }
            else
            {
                StopAttack();
                Animator.SetBool(nameof(HoldingWeapon),true);
            }
        }
        else if (platform.players.Count==0) 
        {
            StopAttack();
            Animator.SetBool(HoldingWeapon,true);
        }
    }
}
