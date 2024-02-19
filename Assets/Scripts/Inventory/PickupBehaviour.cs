using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class PickupBehaviour : MonoBehaviour
{
    [SerializeField]
    private MoveBehaviour playerMoveBehaviour;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Inventory inventory;
    
    [SerializeField] 
    private GameObject swordVisual;

    [SerializeField] 
    private GameObject equippedSwordVisual;

    private Item currentItem;
    public void DoPickup(Item item)
    {
        currentItem = item;
        playerAnimator.SetTrigger("Pickup");
        playerMoveBehaviour.canMove = false;
        
        currentItem.gameObject.SetActive(false);
        equippedSwordVisual.SetActive(true);
    }
    public void AddItemToInventory()
    {
        inventory.AddItem(currentItem.itemData);
        Destroy(currentItem.gameObject);
        currentItem = null;
    }
    public void ReEnablePlayerMovement()
    {
        playerMoveBehaviour.canMove = true;
    }
}
