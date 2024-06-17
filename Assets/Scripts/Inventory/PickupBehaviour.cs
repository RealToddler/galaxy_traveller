using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
public class PickupBehaviour : MonoBehaviourPun
{
    private MoveBehaviour _moveBehaviour;
    private Animator _animator;
    private Inventory _inventory;
    private Player _player;
    
    private readonly List<Item> _nearItems = new();
    private Item _nearestItem;
    
    private static readonly int Pickup = Animator.StringToHash("Pickup");

    private void Start()
    {
        _moveBehaviour = GetComponent<MoveBehaviour>();
        _animator = GetComponent<Animator>();
        _inventory = GetComponent<Inventory>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (!photonView.IsMine || _player.IsInAction)
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
        if (Input.GetButtonDown("Release") && !_inventory.IsTheCurrSlotFree())
        {
            _inventory.ReleaseItem();
            _inventory.Content[_inventory.ItemIndex] = null;
        }
    }

    // Manage the "picking" up of an item
    private void DoPickup()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        if (!_moveBehaviour.canMove)
        {
            return;
        }
        
        _animator.SetTrigger(Pickup);
        _moveBehaviour.canMove = false;
    }
    
    // Add the item to inventory and destroy it
    public void AddItemToInventory()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        _inventory.AddItem(_nearestItem!.itemData);
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
        
        _moveBehaviour.canMove = true;
    }
}
