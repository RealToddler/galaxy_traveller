using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemData[] Content { get; private set; }
    public static int InventorySize => 4;
    public int ItemIndex { get; private set; }

    [SerializeField] private EquipmentLibrary equipmentLibrary;
    [SerializeField] private Transform player;
    

    private EquipmentLibraryItem _equipmentLibraryItem;

    private void Start()
    {
        Content = new ItemData[4];
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            ItemIndex = ItemIndex != InventorySize-1 ? ItemIndex + 1 : 0;
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            ItemIndex = ItemIndex != 0 ? ItemIndex - 1 : InventorySize - 1;
        }
        
        DisplayItemVisual();
    }

    private void DisplayItemVisual()
    {
        EquipmentLibraryItem nextItem = equipmentLibrary.content.Find(elem => elem.itemData == Content[ItemIndex]);
        if (_equipmentLibraryItem != nextItem)
        {
            _equipmentLibraryItem?.itemPrefab.SetActive(false);
            _equipmentLibraryItem = nextItem;
            _equipmentLibraryItem?.itemPrefab.SetActive(true);
        }
    }

    private bool IsTheCurrSlotFree()
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
            var position = player.position;
            Instantiate(Content[ItemIndex].prefab, new Vector3(position.x, position.y+1, position.z-1), 
                Content[ItemIndex].prefab.transform.rotation);
        }
        
        Content[ItemIndex] = item;
    }
    
    public void RemoveItem()
    {
        Content[ItemIndex] = null;
    }
}