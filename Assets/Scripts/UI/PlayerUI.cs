using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Inventory inventory;

    [SerializeField] private RectTransform healthBarFill;
    [SerializeField] private RectTransform oxygenBarFill;
    [SerializeField] private Transform inventorySlots;
    [SerializeField] private GameObject pauseMenu;
    

    private void Update()
    {
        RefreshHealthAmount();
        RefreshOxygenAmount();
        RefreshInventory();

        if (Input.GetButtonDown("Escape"))
        {
            pauseMenu.gameObject.SetActive(!pauseMenu.activeSelf);
        }
    }
    
    // Refresh Health Bar
    void RefreshHealthAmount()
    {
        // healthBarFill.localScale = new Vector3(1f, player.Health/player.maxHealth, 1f);
    }
    
    // Refresh Oxygen Bar
    void RefreshOxygenAmount()
    {
        // oxygenBarFill.localScale = new Vector3(1f, player.Oxygen/player.maxOxygen, 1f);
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
            else
            {
                inventorySlots.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
            }
        }
        
        // visual of slot
        for (int i = 0; i < Inventory.InventorySize; i++)
        {
            if (i == inventory.ItemIndex)
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
}
