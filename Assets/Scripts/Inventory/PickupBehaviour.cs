using System;
using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{
    [SerializeField] private MoveBehaviour playerMoveBehaviour;

    [SerializeField] private Animator playerAnimator;

    [SerializeField] private Inventory inventory;

    private Item _currentItem;
    
    private static readonly int Pickup = Animator.StringToHash("Pickup");

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            if (Input.GetButtonDown("Action1"))
            {
                print("Collect");
            }
        }
    }

    // Manage the "picking" up of an item
    public void DoPickup(Item item)
    {
        if (!playerMoveBehaviour.canMove)
        {
            return;
        }

        _currentItem = item;
        
        playerAnimator.SetTrigger(Pickup);
        playerMoveBehaviour.canMove = false;
    }
    
    // Add the item to inventory and destroy it
    public void AddItemToInventory()
    {
        inventory.AddItem(_currentItem.itemData);
        Destroy(_currentItem.gameObject);
        _currentItem = null;
    }
    
    // Called by Animator at the end of animations playing
    public void ReEnablePlayerMovement()
    {
        playerMoveBehaviour.canMove = true;
    }
}
