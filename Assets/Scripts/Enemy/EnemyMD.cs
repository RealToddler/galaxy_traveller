using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMD : Enemy
{
    [Header("Melee")]

    [SerializeField] public float RadiusAttackM;
    [SerializeField] public float RadiusApproach;
    [SerializeField] private GameObject _sword;
    [SerializeField] private GameObject _gun;
    private float _distance;
    

    void Start()
    {
        IsAttacking = false;
        Health = _maxHealth;
        _distance=radiusAttack+1;
    }
    void Update()
    {
        if (!IsDead)
        {
            if(Health<=0)
            {
                IAAnimator.SetTrigger("IsDead");
                IAAnimator.SetBool("IsAttacking",false);
                IsDead=true;
            }
            else 
            {
                IsAttacking=IAAnimator.GetBool("IsAttacking");
                AttackManager();
                CheckForChanging();
                IAAnimator.SetBool("StopAttackMelee",_gun.activeSelf || IsAttacking==false);
                IAAnimator.SetBool("StopAttackDistance",_sword.activeSelf || IsAttacking==false);
                IAAnimator.SetBool("IsShaking",_sword.activeSelf);
                IAAnimator.SetBool("HoldingWeapon",_gun.activeSelf && !IsAnimationPlaying("AttackDistance"));
                if (platform.players.Count!=0)_distance=Vector3.Distance(platform.players[IndexNearestPlayer()].position, transform.position);
                _gun.SetActive(_distance>RadiusApproach);
                _sword.SetActive(_distance<=RadiusApproach);
            }
        }
        
    }
    private void CheckForChanging()
    {
        if (platform.players.Count != 0)
        {
            if (_distance<=RadiusApproach && IAAnimator.GetBool("AttackDistance"))
            {
                StopAttackDistance();
            }
            else if (_distance>RadiusApproach && IAAnimator.GetBool("AttackMelee") && _distance<=radiusAttack )
            {
                StopAttackMelee();
            }
        }
    }
    bool IsAnimationPlaying(string animName)
    {
        // Récupère les informations de l'état actuel de l'Animator
        AnimatorStateInfo stateInfo = IAAnimator.GetCurrentAnimatorStateInfo(1);
        // Vérifie si le nom de l'état contient le nom de l'animation
        return stateInfo.IsName(animName);
    }
    private void IncreaseAttack()
    //called in iaattack_distance anim
    {
        nbshots+=1;
    }
    public void StopAttackMelee()
    {
        IAAnimator.SetBool("HoldingWeapon",true);
        IAAnimator.SetBool("IsAttacking",false);
        IAAnimator.SetBool("AttackMelee",false);
        nbshots=0;
    }
    public void StopAttackDistance()
    {
        IAAnimator.SetBool("HoldingWeapon",false);
        IAAnimator.SetBool("IsAttacking",false);
        IAAnimator.SetBool("AttackDistance",false);
        nbshots=0;
    }
    public override void StopAttack()
    {
        IAAnimator.SetBool("IsAttacking",false);
        IAAnimator.SetBool("AttackDistance",false);
        IAAnimator.SetBool("AttackMelee",false);
        IAAnimator.SetBool("StopAttackMelee",true);
        IAAnimator.SetBool("StopAttackDistance",true);
        nbshots=0;
    }
    protected override void FinishAnim()
    {
        if (platform.players.Count!=0)
        {
            if (_distance>radiusAttack )
            {
                StopAttack();
                IAAnimator.SetBool("HoldingWeapon",true);
            }
            else if ( _distance<=RadiusApproach && _distance>RadiusAttackM)
            {
                StopAttack();
            }

        }
    }
    public override void AttackManager()
    {
        if (!IsDead && !IsAttacking && !IAAnimator.GetBool("Backward") && platform.players.Count != 0)
        {
            if (_distance <= RadiusAttackM)
            {
                FindAndLaunchAttack("Melee");
            }
            else if (_distance<=RadiusApproach)
            {

            }
            else if (_distance<=radiusAttack)
            {
                FindAndLaunchAttack("Distance");
            }
            else
            {
                StopAttack();
                IAAnimator.SetBool("HoldingWeapon",true);
            }
        }
        else if (platform.players.Count==0) 
        {
            StopAttack();
            IAAnimator.SetBool("HoldingWeapon",true);
        }
    }
}
