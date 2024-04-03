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
    private readonly int _speedAnim = Animator.StringToHash("Speed");
    private readonly int _drinkAnim = Animator.StringToHash("Drink");
    private readonly int _holdPotion = Animator.StringToHash("HoldPotion");
    private readonly int _holdSword = Animator.StringToHash("HoldSword");
    private readonly int _holdWeapon = Animator.StringToHash("HoldWeapon");
    
    private bool _noMoreO2;
    private bool _isRespawning;
    private bool _isInvincible;

    public int maxHealth = 100;
    public int maxOxygen = 100;

    public float Health { get; private set; }
    public float Oxygen { get; private set; }
    public bool IsInAction { get; private set; }


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
        OxygenManager();
        ActionManager();
        HoldingVisualManager();
    }

    private void ActionManager()
    {
        if (Input.GetButtonDown("Action1") && Health > 0 && Oxygen > 0 && !IsInAction)
        {
            if (inventory.Content[inventory.ItemIndex].IsUnityNull())
            {
                return;
            }

            IsInAction = true;
            
            if (inventory.IsTheCurrSelectedItem("Sword"))
            {
                playerAnimator.SetTrigger(_attackMeleeAnim);
            }
            else if (inventory.IsTheCurrSelectedItem("Weapon"))
            {
                playerAnimator.SetTrigger(_attackDistanceAnim);
            }
            else
            {
                if (inventory.IsTheCurrSelectedItem("HealthPotion"))
                {
                    Health = Health <= 80 ? Health + 20 : maxHealth;
                }
                else if (inventory.IsTheCurrSelectedItem("InvincibilityPotion"))
                {
                    if (!_isInvincible)
                    {
                        _isInvincible = true;
                        // gold invincibility screen
                        Invoke(nameof(SetInvincibleToFalse), 5f);
                    }
                }
                else if (inventory.IsTheCurrSelectedItem("OxygenPotion"))
                {
                    Oxygen = Oxygen <= 90 ? Oxygen + 10 : maxOxygen;
                }
                
                playerAnimator.SetTrigger(_drinkAnim);
            }
        }
    }

    private void HoldingVisualManager()
    {
        if (inventory.Content[inventory.ItemIndex].IsUnityNull())
        {
            playerAnimator.SetBool(_holdWeapon,false);
            playerAnimator.SetBool(_holdPotion,false);
            playerAnimator.SetBool(_holdSword,false);
        }
        else
        {
            playerAnimator.SetBool(_holdSword, inventory.IsTheCurrSelectedItem("Sword"));
            playerAnimator.SetBool(_holdWeapon, inventory.IsTheCurrSelectedItem("Weapon"));
            playerAnimator.SetBool(_holdPotion, inventory.Content[inventory.ItemIndex].name is "HealthPotion" or 
                "InvincibilityPotion" or "OxygenPotion");
        }
    }
    
    // Remove qty of O2 to player Oxygen
    private void OxygenManager()
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
                playerAnimator.SetFloat(_speedAnim, 0);
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
        
        if (Health - damage > 0)
        {
            Health -= damage;
        }
        else
        {
            Health = 0;
            
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
        IsInAction = false;
        playerAnimator.SetLayerWeight(6,1);
    }

    public void ConsumeItem()
    {
        inventory.RemoveItem();
    }
}