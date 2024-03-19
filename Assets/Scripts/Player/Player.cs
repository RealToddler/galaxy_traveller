using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private MoveBehaviour moveBehaviour;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private LayerMask layerMask;
    
    private int _attackMelee = Animator.StringToHash("Attack");
    private int deadPv = Animator.StringToHash("DiePv");
    private int deadOxy = Animator.StringToHash("DieOxy");
    
    private bool isAttacking;
    private bool isDeadPv;
    private bool isDeadOxy;
    private bool isRespawning;
    
    public int maxHealth = 100;
    public int maxOxygen = 100;

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
        LooseOxygen();
        DiePv();
        DieOxy();
        AnimationManager();
        GetDamage(0.04f);
        AttackManager();
    }

    public void AttackManager()
    {
        Debug.DrawRay(transform.position + new Vector3(0,1,0), transform.forward *2.2f, Color.white);
        if (!isDeadPv && !isAttacking && Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            SendAttackMelee();
            playerAnimator.SetTrigger(_attackMelee);
            Invoke(nameof(SetAttackingToFalse), 0.5f);
        }
    }

    public void SendAttackMelee()
    {
        Debug.Log("Attack sent");

        RaycastHit hit;

        if (Physics.Raycast(transform.position + new Vector3(0,1,0), transform.forward, out hit, 2.2f, layerMask))
        {
            if (hit.transform.CompareTag("AI"))
            {
                hit.collider.GetComponent<Ennemy>().LooseHealth(50f);
            }
        }
    }

    public void AnimationManager()
    {
        
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
        isAttacking = false;
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