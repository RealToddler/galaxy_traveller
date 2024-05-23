using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{
    [SerializeField] private MoveBehaviour playerMoveBehaviour;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Inventory inventory;
    [SerializeField] private float range = 2f;

    private List<Item> nearItems = new();
    [CanBeNull] private Item _currentItem;
    
    private static readonly int Pickup = Animator.StringToHash("Pickup");

    private void Update()
    {
        UpdateNearest();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            nearItems.Add(other.gameObject.GetComponent<Item>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            nearItems.Remove(other.gameObject.GetComponent<Item>());
        }
    }

    void UpdateNearest()
    {
        if (nearItems.Count > 0)
        {
            Item nearestItem = nearItems
                .OrderByDescending(item => Vector3.Distance(transform.position, item.transform.position))
                .FirstOrDefault(item => Vector3.Distance(item.transform.position, transform.position) <= range);
            
            if (Input.GetButtonDown("Action2"))
            {
                DoPickup(nearestItem!.GetComponent<Item>());
                nearItems.Remove(nearestItem);
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
