using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] public RectTransform healthBarFill;
    [SerializeField] private RectTransform oxygenBarFill;
    [SerializeField] private Transform inventorySlots;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Text itemsTips;
    [SerializeField] private Text globalTips;
    [SerializeField] private GameObject sight;
    [SerializeField] public RectTransform bossBarFill;
    [SerializeField] private GameObject bossBar;
    [SerializeField] public Text bossName;


    private Player _player;
    private Inventory _inventory;
    private Dictionary<string, string> _inputKey;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        
        _inputKey = new Dictionary<string, string>();
        ReadAxes();
        globalTips.text = $"Press E to collect\n" +
                          $"Press Q to release";
    }

    private void Update()
    {
        RefreshHealthAmount();
        RefreshOxygenAmount();
        RefreshInventory();
        RefreshTips();

        if (Input.GetButtonDown("Escape"))
        {
            ChangePauseMenuState();
        }
        
        if (_player.IsAiming)
        {
            if (!sight.activeSelf)
            {
                sight.gameObject.SetActive(true);
            }
        }
        else
        {
            sight.gameObject.SetActive(false);
        }
        
        // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
        if (_player == null)
        {
            Destroy(gameObject);
        }
    }

    private void RefreshTips()
    {
        ItemData currentItem = _inventory.Content[_inventory.ItemIndex];

        if (!currentItem)
        {
            itemsTips.text = "";
        }
        else
            itemsTips.text = 
                currentItem.prefab.name switch 
                { 
                    "HealthPotion" or "InvincibilityPotion" or "OxygenPotion" => "Click LEFT to drink", 
                    "MoonSword" => "Click LEFT to attack", 
                    _ => ""
                };
    }

    // Refresh Health Bar
    private void RefreshHealthAmount()
    {
        healthBarFill.localScale = new Vector3(1f, _player.Health/_player.maxHealth, 1f);
    }
    
    // Refresh Oxygen Bar
    private void RefreshOxygenAmount()
    {
        oxygenBarFill.localScale = new Vector3(1f, _player.Oxygen/_player.maxOxygen, 1f);
    }
    
    // Refresh Boss Bar
    public void RefreshBossAmount(float bossHealth, bool activate)
    {
        if (activate && !bossBar.gameObject.activeSelf)
        {
            bossBar.gameObject.SetActive(true);
        }

        if (!activate && bossBar.gameObject.activeSelf)
        {
            bossBar.gameObject.SetActive(false);
        }
        
        if (activate)
        {
            bossBarFill.localScale = new Vector3(1f, bossHealth/100, 1f);
        }
    }
    
    // Refresh the visual of inventory
    private void RefreshInventory()
    {
        Color normalColor = inventorySlots.GetChild(0).GetComponent<Button>().colors.normalColor;
        Color selectedColor = inventorySlots.GetChild(0).GetComponent<Button>().colors.selectedColor;

        // visual of items
        for (int i = 0; i < _inventory.Content.Length; i++)
        {
            if (_inventory.Content[i] != null)
            {
                if (!inventorySlots.GetChild(i).GetChild(0).GetComponent<Image>().gameObject.activeSelf)
                {
                    inventorySlots.GetChild(i).GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
                }
                inventorySlots.GetChild(i).GetChild(0).GetComponent<Image>().sprite = _inventory.Content[i].visual;
            }
            else
            {
                inventorySlots.GetChild(i).GetChild(0).GetComponent<Image>().gameObject.SetActive(false);
            }
        }
        
        // visual of slot
        for (int i = 0; i < Inventory.InventorySize; i++)
        {
            if (i == _inventory.ItemIndex)
            {
                inventorySlots.GetChild(i).GetComponent<Image>().color = selectedColor;
            }
            else
            {
                inventorySlots.GetChild(i).GetComponent<Image>().color = normalColor;
            }
        }
    }

    public void ChangePauseMenuState()
    {
        pauseMenu.gameObject.SetActive(!pauseMenu.activeSelf);
        _player.IsInAction = pauseMenu.activeSelf;
        Cursor.visible = pauseMenu.activeSelf;
    }

    public void BackToMainMenu()
    {
        Debug.Log("back");
        SceneManager.LoadScene("Menus");
    }

    private void ReadAxes()
    {
        // To complete
    }
    public void SetTarget(PlayerManager target)
    {
        if (target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;

        }
        // Cache references for efficiency
        _player = target.GetComponent<Player>();
        _inventory = target.GetComponent<Inventory>();
    }
}
