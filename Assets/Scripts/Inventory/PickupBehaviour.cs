#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public class PickupBehaviour : MonoBehaviourPun
{
    [SerializeField] private MoveBehaviour playerMoveBehaviour;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Inventory inventory;
    
    private readonly List<Item> _nearItems = new();
    private Item? _nearestItem;
    
    private static readonly int Pickup = Animator.StringToHash("Pickup");

    private void Update()
    {
        if (!photonView.IsMine || GetComponent<Player>().IsInAction)
        {
            return;
        }
        
        PickUpManager();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        if (other.gameObject.CompareTag("Item"))
        {
            _nearItems.Add(other.gameObject.GetComponent<Item>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        if (other.gameObject.CompareTag("Item"))
        {
            _nearItems.Remove(other.gameObject.GetComponent<Item>());
        }
    }

    void PickUpManager()
    {
        if (Input.GetButtonDown("Collect") && _nearItems.Count > 0)
        {
            _nearestItem = _nearItems
                .OrderByDescending(item => Vector3.Distance(transform.position, item.transform.position))
                .First();

            DoPickup();
            
            _nearItems.Remove(_nearestItem);
        }
        if (Input.GetButtonDown("Release") && !inventory.IsTheCurrSlotFree())
        {
            inventory.ReleaseItem();
            inventory.Content[inventory.ItemIndex] = null;
        }
    }

    // Manage the "picking" up of an item
    private void DoPickup()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        if (!playerMoveBehaviour.canMove)
        {
            return;
        }
        
        playerAnimator.SetTrigger(Pickup);
        playerMoveBehaviour.canMove = false;
    }
    
    // Add the item to inventory and destroy it
    public void AddItemToInventory()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        inventory.AddItem(_nearestItem!.itemData);
        _nearestItem.CollectItem();
        _nearestItem = null;
    }
    
    // Called by Animator at the end of animations playing
    public void ReEnablePlayerMovement()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        playerMoveBehaviour.canMove = true;
    }
}
