using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{
    [SerializeField] private MoveBehaviour playerMoveBehaviour;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Inventory inventory;
    [SerializeField] private float range = 2f;

    private Item _currentItem;
    
    private static readonly int Pickup = Animator.StringToHash("Pickup");

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void Update()
    {
        UpdateTarget();
    }

    void UpdateTarget()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        GameObject nearestItem = items
            .OrderByDescending(item => Vector3.Distance(transform.position, item.transform.position))
            .FirstOrDefault(item => Vector3.Distance(item.transform.position, transform.position) <= range);
        
        if (nearestItem != null && Input.GetButtonDown("Fly"))
        {
            DoPickup(nearestItem.GetComponent<Item>());
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
