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
        RefreshInventory();
        
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
    private void RefreshInventory()
    {
        Color normalColor = inventorySlots.GetChild(0).GetComponent<Button>().colors.normalColor;
        Color selectedColor = inventorySlots.GetChild(0).GetComponent<Button>().colors.selectedColor;

        // visual of items
        for (int i = 0; i < inventory.Content.Length; i++)
        {
            if (inventory.Content[i] != null)
            {
                inventorySlots.GetChild(i).GetChild(0).GetComponent<Image>().sprite = inventory.Content[i].visual;
            }
        }
        
        // visual of slot
        for (int i = 0; i < Inventory.InventorySize; i++)
        {
            if (i == inventory.CurrSelectedItem)
            {
                inventorySlots.GetChild(i).GetComponent<Image>().color = selectedColor;
            }
            else
            {
                inventorySlots.GetChild(i).GetComponent<Image>().color = normalColor;
            }
        }
    }
}
