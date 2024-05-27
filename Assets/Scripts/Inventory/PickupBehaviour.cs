#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

public class PickupBehaviour : MonoBehaviour
{
    [SerializeField] private MoveBehaviour playerMoveBehaviour;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Inventory inventory;
    
    private readonly List<Item> _nearItems = new();
    private Item? _nearestItem;

    private PhotonView _view;
    
    private static readonly int Pickup = Animator.StringToHash("Pickup");

    private void Start()
    {
        _view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!_view.IsMine)
        {
            return;
        }
        
        PickUpManager();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_view.IsMine)
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
        if (!_view.IsMine)
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
        if (Input.GetButtonDown("Action2") && _nearItems.Count > 0)
        {
            print(_nearItems.Count);
            _nearestItem = _nearItems
                .OrderByDescending(item => Vector3.Distance(transform.position, item.transform.position))
                .First();

            print(_nearestItem);
            DoPickup();
            
            _nearItems.Remove(_nearestItem);
            print(_nearItems.Count);
        }
    }

    // Manage the "picking" up of an item
    private void DoPickup()
    {
        if (!_view.IsMine)
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
        if (!_view.IsMine)
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
        if (!_view.IsMine)
        {
            return;
        }
        
        playerMoveBehaviour.canMove = true;
    }
}
