using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PhotonView view;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private MoveBehaviour moveBehaviour;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Inventory inventory;
    
    private readonly int _attackMeleeAnim = Animator.StringToHash("Attack");
    private readonly int _attackDistanceAnim = Animator.StringToHash("AttackDistance");
    private readonly int _deadHpAnim = Animator.StringToHash("DeadHp");
    private readonly int _deadO2Anim = Animator.StringToHash("DeadO2");
    private readonly int _speed = Animator.StringToHash("Speed");
    private readonly int _drink = Animator.StringToHash("Drink");
    private readonly int _hasPotion = Animator.StringToHash("HasPotion");
    private readonly int _hasSword = Animator.StringToHash("HasSword");
    private readonly int _holdWeapon = Animator.StringToHash("HoldWeapon");


    
    private bool _isInAction;
    private bool _noMoreO2;
    private bool _isRespawning;
    private bool _isInvincible;

    public int maxHealth = 100;
    public int maxOxygen = 100;

    public float Health { get; private set; }
    public float Oxygen { get; private set; }

    private void Start()
    {
        // Proprieties initialisation
        Health = maxHealth;
        Oxygen = maxOxygen;

        // Oxygen = 1;
        
        // Network
        //view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        LooseOxygen();
        ActionManager();
    }

    private void ActionManager()
    {
        if (Input.GetButtonDown("Action1") && Health > 0 && !_isInAction)
        {
            if (inventory.Content[inventory.ItemIndex].IsUnityNull())
            {
                return;
            }

            _isInAction = true;
            
            if (inventory.IsTheCurrSelectedItem("Sword"))
            {
                playerAnimator.SetTrigger(_attackMeleeAnim);
            }
            if (inventory.IsTheCurrSelectedItem("Weapon"))
            {
                Debug.Log("weapon");
                playerAnimator.SetLayerWeight(6,0);
                playerAnimator.SetTrigger(_attackDistanceAnim);
            }
            else if (inventory.IsTheCurrSelectedItem("HealthPotion"))
            {
                Health = Health <= 80 ? Health + 20 : maxHealth;
                
                playerAnimator.SetTrigger(_drink);
            }
            else if (inventory.IsTheCurrSelectedItem("InvincibilityPotion"))
            {
                if (!_isInvincible)
                {
                    _isInvincible = true;
                    Invoke(nameof(SetInvincibleToFalse), 5f);
                }
                playerAnimator.SetTrigger(_drink);
            }
            else if (inventory.IsTheCurrSelectedItem("OxygenPotion"))
            {
                Oxygen = Oxygen <= 90 ? Oxygen + 10 : maxOxygen;
                
                playerAnimator.SetTrigger(_drink);
            }
        }
        
        if (inventory.Content[inventory.ItemIndex].IsUnityNull())
        {
            playerAnimator.SetBool(_holdWeapon,false);
            playerAnimator.SetBool(_hasPotion,false);
            playerAnimator.SetBool(_hasSword,false);
        }
        else if (inventory.Content[inventory.ItemIndex].name=="Weapon")
        {
            playerAnimator.SetBool(_holdWeapon,true);
            playerAnimator.SetBool(_hasSword,false);
            playerAnimator.SetBool(_hasPotion,false);
        }
        else if (inventory.Content[inventory.ItemIndex].name=="Sword")
        {
            playerAnimator.SetBool(_hasSword,true);
            playerAnimator.SetBool(_holdWeapon,false);
            playerAnimator.SetBool(_hasPotion,false);
        }
        else 
        {
            playerAnimator.SetBool(_hasPotion,true);
            playerAnimator.SetBool(_holdWeapon,false);
            playerAnimator.SetBool(_hasSword,false);
        }
    }
    
    // Remove qty of O2 to player Oxygen
    private void LooseOxygen()
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
            if (!_isRespawning)
            {
                _isRespawning = true;
                moveBehaviour.canMove = false;
                playerAnimator.SetFloat(_speed, 0);
                playerAnimator.SetTrigger(_deadO2Anim);
            }
        }
    }
    
    // Remove damage to player health
    public void TakeDamage(float damage)
    {
        if (_isInvincible)
        {
            return;
        }
        
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

    private void SetInvincibleToFalse()
    {
        _isInvincible = false;
    }

    // ==================== All functions called in actions animations ====================
    
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
    
    public void RespawnAfterDeathHp()
    {
        Health = maxHealth;
        _isRespawning = false;
        transform.position = respawnPoint.transform.position;
        moveBehaviour.enabled = true;
    }

    // Call the game over menu
    public void ToGameOverScreen()
    {
        SceneManager.LoadScene("GameOver");
    }
    
    public void SetInActionToFalse()
    {
        _isInAction = false;
        playerAnimator.SetLayerWeight(6,1);


    }

    public void ConsumeItem()
    {
        inventory.RemoveItem();
    }
}