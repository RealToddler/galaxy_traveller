using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    public PhotonView view;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private MoveBehaviour moveBehaviour;
    [SerializeField] private Transform respawnPoint;
    
    private readonly int _attackMeleeAnim = Animator.StringToHash("Attack");
    private readonly int _deadHpAnim = Animator.StringToHash("DeadHp");
    private readonly int _deadO2Anim = Animator.StringToHash("DeadO2");
    
    private bool _isAttacking;
    private bool _noMoreO2;
    private bool _isRespawning;

    public int maxHealth = 100;
    public int maxOxygen = 100;

    public float Health { get; private set; }
    public float Oxygen { get; private set; }

    private void Start()
    {
        // Proprieties initialisation
        Health = maxHealth;
        Oxygen = maxOxygen;
        
        // Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        
        // Network
        //view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        //if (view.IsMine)
        {
            LooseOxygen();
            AttackManager();
            // TakeDamage(0.3f);
        }
    }

    public void AttackManager()
    {
        if (Input.GetButtonDown("Fire1") && Health > 0 && !_isAttacking)
        {
            _isAttacking = true;
            playerAnimator.SetTrigger(_attackMeleeAnim);
        }
    }

    public void SendAttackMelee()
    {
        Debug.Log("Attack sent");

        RaycastHit hit;

        if (Physics.Raycast(transform.position + new Vector3(0,1,0), transform.forward, out hit, 2.2f))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Enemy>().LooseHealth(25f);
            }
        }
    }
    
    // Remove damage to player health
    public void TakeDamage(float damage)
    {
        if (Health > 0)
        {
            Health -= damage;
        }
        else
        {
            if (!_isRespawning)
            {
                moveBehaviour.enabled = false;
                _isRespawning = true;
                playerAnimator.SetTrigger(_deadHpAnim);
            }
        }
    }
    
    //This function manages the things to reset normally after the player's death and it brings the player to the respawn point.
    public void RespawnAfterDeathHp()
    {
        Health = maxHealth;
        _isRespawning = false;
        transform.position = respawnPoint.transform.position;
        moveBehaviour.enabled = true;
    }

    // Remove qty of O2 to player Oxygen
    public void LooseOxygen()
    {
        if (Oxygen >= 10)
        {
            Oxygen -= 0.007f;
        }
        else if (Oxygen is < 10 and > 0) 
        {
            Oxygen -= 0.002f;
            playerAnimator.speed = 0.7f;
        } 
        else 
        {
            playerAnimator.SetTrigger(_deadO2Anim);
            Debug.Log("Game Over");
        }
    }
    
    public void SetAttackingToFalse()
    {
        _isAttacking = false;
    }
}