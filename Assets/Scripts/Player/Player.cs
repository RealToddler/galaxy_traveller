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
    private bool _isRespawning;
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

    public int maxHealth = 100;
    public int maxOxygen = 100;

    public float Health { get; private set; }
    public float Oxygen { get; private set; }
    public bool IsInAction { get; set; }
    public bool IsAiming { get; set; }
    
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
        if (Input.GetButtonDown("Action1") && Health > 0 && Oxygen > 0 && !IsInAction && _moveBehaviour.IsGrounded())
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
            }
            else if (_inventory.IsTheCurrSelectedItem("Weapon"))
            {
                LaunchTriggerAnim(_attackDistanceAnim);
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
            if (!_isRespawning)
            {
                _isRespawning = true;
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
        else if (Health < 100)
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
            
            if (!_isRespawning)
            {
                _moveBehaviour.enabled = false;
                _isRespawning = true; 
                LaunchTriggerAnim(_deadHpAnim);
            }
        }
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
    
    public void SendAttackMelee()
    {
        Debug.Log("Attack sent");

        if (Physics.Raycast(transform.position + new Vector3(0,1,0), transform.forward, out var hit, 2.2f))
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
        _playerAnimator.SetLayerWeight(6,1);
    }

    public void ConsumeItem()
    {
        _inventory.RemoveItem();
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