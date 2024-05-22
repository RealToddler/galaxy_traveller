using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private RectTransform healthBarFill;
    [SerializeField] private RectTransform oxygenBarFill;
    [SerializeField] private Transform inventorySlots;
    [SerializeField] private GameObject pauseMenu;

    private Player _player;
    private Inventory _inventory;

    private void Update()
    {
        RefreshHealthAmount();
        RefreshOxygenAmount();
        RefreshInventory();

        if (Input.GetButtonDown("Escape"))
        {
            pauseMenu.gameObject.SetActive(!pauseMenu.activeSelf);
        }
        
        // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
        if (_player == null)
        {
            Destroy(this.gameObject);
            return;
        }
    }
    
    // Refresh Health Bar
    void RefreshHealthAmount()
    {
        healthBarFill.localScale = new Vector3(1f, _player.Health/_player.maxHealth, 1f);
    }
    
    // Refresh Oxygen Bar
    void RefreshOxygenAmount()
    {
        oxygenBarFill.localScale = new Vector3(1f, _player.Oxygen/_player.maxOxygen, 1f);
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
                inventorySlots.GetChild(i).GetChild(0).GetComponent<Image>().sprite = _inventory.Content[i].visual;
            }
            else
            {
                inventorySlots.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
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

    public void BackToMainMenu()
    {
        Debug.Log("back");
        SceneManager.LoadScene("Menus");
    }
    
    public void SetTarget(PlayerManager _target)
    {
        if (_target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }
        // Cache references for efficiency
        _player = _target.GetComponent<Player>();
        _inventory = _target.GetComponent<Inventory>();
    }
}
