using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks
{
    private MoveBehaviour _moveBehaviour;
    private PhotonView _view;
    private Inventory _inventory;
    private Animator _playerAnimator;
    private Vector3 _respawnPoint;
    private bool _noMoreO2;
    private bool _isInvincible;
    private GameObject _ui;
    private PostProcessVolume _volume;
    private Vignette _vignette;
    
    private readonly int _attackMeleeAnim = Animator.StringToHash("Attack");
    private readonly int _attackDistanceAnim = Animator.StringToHash("AttackDistance");
    private readonly int _deadHpAnim = Animator.StringToHash("DeadHp");
    private readonly int _deadO2Anim = Animator.StringToHash("DeadO2");
    private readonly int _speedAnim = Animator.StringToHash("Speed");
    private readonly int _drinkAnim = Animator.StringToHash("Drink");
    private readonly int _holdPotion = Animator.StringToHash("HoldPotion");
    private readonly int _holdSword = Animator.StringToHash("HoldSword");
    private readonly int _holdWeapon = Animator.StringToHash("HoldWeapon");
    private readonly int _knockback = Animator.StringToHash("Knockback");

    public int maxHealth = 100;
    public int maxOxygen = 100;


    public float Health { get; private set; }
    public float Oxygen { get; private set; }
    public bool IsInAction { get; set; }
    public bool IsAiming { get; set; }
    public bool HasHit { get; set; }

    public bool IsRespawning;
    public bool IsHit;
    [HideInInspector]
    public bool CanAttack;
    [Header("Attack Distance")]
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _eject;
    
    private void Start()
    {
        Health = maxHealth;
        Oxygen = maxOxygen;
        
        _view = GetComponent<PhotonView>();
        _inventory = GetComponent<Inventory>();
        _playerAnimator = GetComponent<Animator>();
        _moveBehaviour = GetComponent<MoveBehaviour>();
        _respawnPoint = transform.position;
        _ui = gameObject.GetComponent<PlayerManager>().ui;
        _volume = Camera.main!.GetComponentInChildren<PostProcessVolume>();
        _volume.profile.TryGetSettings(out _vignette);
    }

    private void Update()
    {
        if (_view.IsMine) 
        {
            OxygenManager();
            HealthManager();
            ActionManager();
            HoldingVisualManager();
        }
    }

    private void ActionManager()
    {
        if (Input.GetButtonDown("Action1") && Health > 0 && Oxygen > 0 && !IsHit && !IsInAction && _moveBehaviour.IsGrounded())
        {
            if (_inventory.Content[_inventory.ItemIndex].IsUnityNull())
            {
                return;
            }

            IsInAction = true;
            
            if (_inventory.IsTheCurrSelectedItem("MoonSword") || 
                _inventory.IsTheCurrSelectedItem("IceSword") || 
                _inventory.IsTheCurrSelectedItem("FireSword"))
            {
                LaunchTriggerAnim(_attackMeleeAnim);
                HasHit=true;
                Invoke(nameof(SetInActionToFalse),1.2f);
            }
            else if (_inventory.IsTheCurrSelectedItem("Weapon"))
            {
                LaunchTriggerAnim(_attackDistanceAnim);
                Invoke(nameof(SetInActionToFalse),1.2f);
            }
            else
            {
                if (_inventory.IsTheCurrSelectedItem("HealthPotion"))
                {
                    Health = Health <= 80 ? Health + 20 : maxHealth;
                }
                else if (_inventory.IsTheCurrSelectedItem("InvincibilityPotion"))
                {
                    if (!_isInvincible)
                    {
                        _isInvincible = true;
                        
                        // gold invincibility screen
                        _vignette.color.value = new Color(1,1,0);
                        _vignette.intensity.value = 0.4f;
                        
                        Invoke(nameof(SetInvincibleToFalse), 15f);
                    }
                }
                else if (_inventory.IsTheCurrSelectedItem("OxygenPotion"))
                {
                    Oxygen = Oxygen <= 90 ? Oxygen + 10 : maxOxygen;
                }
                
                LaunchTriggerAnim(_drinkAnim);
            }
        }
        
        IsAiming = Input.GetKey(KeyCode.Mouse1) && _inventory.IsTheCurrSelectedItem("Weapon");
    }

    private void HoldingVisualManager()
    {
        if (_inventory.Content[_inventory.ItemIndex].IsUnityNull())
        {
            _playerAnimator.SetBool(_holdWeapon,false);
            _playerAnimator.SetBool(_holdPotion,false);
            _playerAnimator.SetBool(_holdSword,false);
        }
        else
        {
            _playerAnimator.SetBool(_holdSword, _inventory.IsTheCurrSelectedItem("MoonSword") || 
                                                _inventory.IsTheCurrSelectedItem("IceSword") || 
                                                _inventory.IsTheCurrSelectedItem("FireSword"));
            _playerAnimator.SetBool(_holdWeapon, _inventory.IsTheCurrSelectedItem("Weapon"));
            _playerAnimator.SetBool(_holdPotion, _inventory.Content[_inventory.ItemIndex].name is 
                "HealthPotion" or 
                "InvincibilityPotion" or 
                "OxygenPotion");
        }
    }
    
    // Remove qty of O2 to player Oxygen
    private void OxygenManager()
    {
        if (Oxygen >= 10)
        {
            Oxygen -= 0.004f;
        }
        else if (Oxygen is < 10 and > 0) 
        {
            Oxygen -= 0.002f;
            _playerAnimator.speed = 0.8f;
        } 
        else 
        {
            if (!IsRespawning)
            {
                IsRespawning = true;
                _moveBehaviour.canMove = false;
                _playerAnimator.SetFloat(_speedAnim, 0);
                LaunchTriggerAnim(_deadO2Anim);
            }
        }
    }

    private void HealthManager()
    {
        if (Health is > 0 and <= 25)
        {
            Health += 0.01f;
        }
        else if (Health is > 25 and <= 50)
        {
            Health += 0.005f;
        }
        else if (Health is >50 and < 100)
        {
            Health += 0.001f;
        }

        if (!_isInvincible)
        {
            _vignette.intensity.value = 0.6f - Health / maxHealth;
        }
    }

    // Remove damage to player health
    public void TakeDamage(float damage)
    {
        CanAttack = false;
        if (_isInvincible)
        {
            return;
        }
        
        if (Health - damage > 0)
        {
            Health -= damage;
            _ui.GetComponent<PlayerUI>().healthBarFill.gameObject.GetComponent<Image>().color = Color.red;
            Invoke(nameof(BackToGreen), 1);
        }
        else
        {
            Health = 0;
            Oxygen -= Oxygen/4;
            
            if (!IsRespawning)
            {
                _moveBehaviour.enabled = false;
                IsRespawning = true; 
                LaunchTriggerAnim(_deadHpAnim);
            }
        }
    }
    public void KnockBack(float damage)
    {
        IsHit=true;
        LaunchTriggerAnim(_knockback);
        TakeDamage(damage);
    }

    public void BackToGreen()
    {
        _ui.GetComponent<PlayerUI>().healthBarFill.gameObject.GetComponent<Image>().color = new Color(0f, 0.78f, 0.05f, 0.8f);
    }

    private void SetInvincibleToFalse()
    {
        _isInvincible = false;
        _vignette.intensity.value = 0;
        _vignette.color.value = new Color(0.7f, 0.05f,0.05f);
    }


    public void Respawn()
    {
        transform.position = _respawnPoint;
    }

    // ==================== All functions called in actions animations ====================
    
    public void SendAttackDistance()
    {
        Debug.Log("Attack Distance sent");
        GameObject curr=Instantiate(_projectile, _eject.position, _eject.rotation);
        curr.GetComponent<Rigidbody>().velocity=transform.forward*50;    
    }
    
    public void RespawnAfterDeathHp()
    {
        Health = maxHealth;
        IsRespawning = false;
        Respawn();
        _moveBehaviour.enabled = true;
    }

    // Call the game over menu
    public void ToGameOverScreen()
    {
        SceneManager.LoadScene("GameOver");
    }
    
    public void SetInActionToFalse()
    {
        IsInAction = false;
        HasHit=false;
        _playerAnimator.SetLayerWeight(6,1);
    }

    public void ConsumeItem()
    {
        _inventory.RemoveItem();
    }
    public void ResetKnockback()
    {
        IsHit = false;
    }
    private void SetCanAttackTrue()
    {
        CanAttack = true;
    }
    private void SetCanAttackFalse()
    {
        CanAttack = false;
    }
    
    // ==================== Trigger animations synchronisation ====================
    private void LaunchTriggerAnim(int anim)
    {
        photonView.RPC("LaunchIt", RpcTarget.AllBuffered, anim);  
    }
    [PunRPC]
    private void LaunchIt(int anim)
    {
        _playerAnimator.SetTrigger(anim);
    }
}