using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.SlotRacer;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Inventory inventory;

    [SerializeField] private RectTransform healthBarFill;
    [SerializeField] private RectTransform oxygenBarFill;
    [SerializeField] private Transform inventorySlots;

    private void Update()
    {
        RefreshHealthAmount();
        RefreshOxygenAmount();
        RefreshInventoryContent();
        
        player.GetDamage(0.1f);
        player.LooseOxygen(0.05f);
    }
    
    // Refresh Health Bar
    void RefreshHealthAmount()
    {
        healthBarFill.localScale = new Vector3(1f, player.Health/player.maxHealth, 1f);
    }
    
    // Refresh Oxygen Bar
    void RefreshOxygenAmount()
    {
        oxygenBarFill.localScale = new Vector3(1f, player.Oxygen/player.maxOxygen, 1f);
    }
    
    // Refresh the visual of inventory
    private void RefreshInventoryContent()
    {
        for (int i = 0; i < inventory.Content.Count; i++)
        {
            inventorySlots.GetChild(i).GetChild(0).GetComponent<Image>().sprite = inventory.Content[i].visual;
        }
    }
}
