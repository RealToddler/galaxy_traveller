using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100000;
    [SerializeField] public int maxOxygen = 100;
    [SerializeField] private Animator playerAnimator;
    private int attackMelee = Animator.StringToHash("Attack");
    private bool is_attacking;
    private int deadPv = Animator.StringToHash("DiePv");
    private int deadOxy = Animator.StringToHash("DieOxy");
    private bool isDeadPv = false;
    private bool isDeadOxy = false;
    [SerializeField] private MoveBehaviour moveBehaviour;
    [SerializeField] private Transform respawnPoint;
    private bool isRespawning = false;


    public string NickName { get; }

    //public int IsDistanceAttacking;

    public float Health { get; private set; }
    public float Oxygen { get; private set; }

    private void Start()
    {
        Health = maxHealth;
        Oxygen = maxOxygen;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        //LooseOxygen();
        //DiePv();
        //DieOxy();
        AnimationManager();
        // GetDamage(0.04f);
    }

    public void AnimationManager()
    {
        if (!isDeadPv && !is_attacking && Input.GetButtonDown("Fire1"))
        {
            is_attacking = true;
            playerAnimator.SetTrigger(attackMelee);
            Invoke(nameof(SetAttackingToFalse), 0.5f);
        }
        if (isDeadPv)
        {
            playerAnimator.SetBool(deadPv,true);
            isDeadPv = false;
        }

        if (isDeadOxy)
        {
            playerAnimator.SetBool(deadOxy, true);
            isDeadOxy = false;
        }

    }
    public void SetAttackingToFalse()
    {
        is_attacking = false;
    }
    public void DiePv()
    {
        //fct that checks if player has any more pvs. If it is not the case, player is dead and 
        //the function stops every movement of the player and calls the respawning function.
        if (!isDeadPv && Health <= 0)
        {
            isDeadPv = true;
        }
        if (isDeadPv)
        {
            moveBehaviour.enabled = false;
            if (!isRespawning)
            {
                Invoke(nameof(RespawnAfterdeathPv), 4);
                isRespawning = true;
            }
        }
    }

    public void DieOxy() {
        if (!isDeadOxy && Oxygen <= 0) {
            isDeadOxy = true;
        }

        if (isDeadOxy) {
            moveBehaviour.enabled = false;
            if (!isRespawning) {
                Invoke(nameof(RespawnAfterDeathOxy), 4);
                isRespawning = true;
            }
        }
    }
    public void RespawnAfterDeathOxy(){
        playerAnimator.SetBool(deadOxy, false);
        Oxygen=maxOxygen;
        isRespawning=false;
        transform.position = respawnPoint.transform.position;
        moveBehaviour.enabled = true;
    }
    public void RespawnAfterdeathPv()
    //This function manages the things to reset normally after the player's death and it brings the player to the respawn point.
    {
        playerAnimator.SetBool(deadPv, false);
        Health = maxHealth;
        // isDeadPv = false;
        isRespawning = false;
        transform.position = respawnPoint.transform.position;
        moveBehaviour.enabled = true;

    }

    // Remove damage to player health
    public void GetDamage(float damage)
    {
        if (Health > 0)
        {
            Health -= damage;
        }
        else
        {
            // Respawn or GameOver
        }
    }

    // Remove qty of O2 to player Oxygene
    public void LooseOxygen()
    {
        if (Oxygen >= 10)
        {
            Oxygen -= 0.007f;
        }
        else if (Oxygen <= 10 && Oxygen > 0) {
            Oxygen -= 0.002f;
            playerAnimator.speed = 0.7f;
        } else {
            Debug.Log("dead");
        }
    }


}