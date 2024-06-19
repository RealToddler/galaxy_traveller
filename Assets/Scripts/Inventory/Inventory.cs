using System;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviourPunCallbacks, IPunObservable
{
    public ItemData[] Content { get; private set; }
    public static int InventorySize => 4;
    public int ItemIndex { get; private set; }

    [SerializeField] private EquipmentLibrary equipmentLibrary;
    
    private Player _player;
    
    private int _currItem;
    private int _nextItem;

    private void Start()
    {
        Content = new ItemData[4];
        _player = GetComponent<Player>();
        _currItem = -1;
    }

    private void Update()
    {
        if (!_player.IsInAction)
        {
            if (Input.mouseScrollDelta.y < 0)
            {
                ItemIndex = ItemIndex != InventorySize-1 ? ItemIndex + 1 : 0;
            }
            else if (Input.mouseScrollDelta.y > 0)
            {
                ItemIndex = ItemIndex != 0 ? ItemIndex - 1 : InventorySize - 1;
            }
        }
        
        DisplayItemVisual();
    }
    
    public void DisplayItemVisual()
    {
        _nextItem = equipmentLibrary.content.FindIndex(elem => elem.itemData == Content[ItemIndex]);
        if (_currItem != _nextItem)
        {
            photonView.RPC("UpdateItemVisual", RpcTarget.AllBuffered, _currItem,_nextItem);
            _currItem = _nextItem;
        }
    }

    [PunRPC]
    public void UpdateItemVisual(int currItem, int nextItem)
    {
        if (currItem != -1)
        {
            equipmentLibrary.content[currItem].itemPrefab.SetActive(false);
        }

        if (nextItem != -1)
        {
            equipmentLibrary.content[nextItem].itemPrefab.SetActive(true);
        }
    }
    

    public bool IsTheCurrSlotFree()
    {
        return Content.GetValue(ItemIndex).IsUnityNull();
    }

    public bool IsTheCurrSelectedItem(string itemsName)
    {
        return !IsTheCurrSlotFree() && Content[ItemIndex].name == itemsName;
    }

    public void AddItem(ItemData item)
    {
        if (!IsTheCurrSlotFree())
        {
            ReleaseItem();
        }
        
        Content[ItemIndex] = item;
    }
    
    public void RemoveItem()
    {
        Content[ItemIndex] = null;
    }

    public void ReleaseItem()
    {
        var position = _player.transform.position;
        PhotonNetwork.Instantiate(Content[ItemIndex].name, new Vector3(position.x, position.y+1, position.z-1), 
            Content[ItemIndex].prefab.transform.rotation);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}