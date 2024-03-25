using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public ItemData[] Content { get; private set; }

    public static int InventorySize => 4;
    public int ItemIndex { get; private set; }

    [SerializeField]
    private EquipmentLibrary equipmentLibrary;

    private EquipmentLibraryItem equipmentLibraryItem;

    private void Start()
    {
        Content = new ItemData[4];
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            if (!IsTheCurrSlotFree())
            {
                equipmentLibraryItem=equipmentLibrary.content.Where(elem => elem.itemData==Content[ItemIndex]).First();
                equipmentLibraryItem.itemPrefab.SetActive(false);
            }
            if (ItemIndex != InventorySize-1) ItemIndex++;
            else ItemIndex = 0;
            if (!IsTheCurrSlotFree())
            {
                equipmentLibraryItem=equipmentLibrary.content.Where(elem => elem.itemData==Content[ItemIndex]).First();
                equipmentLibraryItem.itemPrefab.SetActive(true);
            }
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            if (!IsTheCurrSlotFree())
            {
                equipmentLibraryItem=equipmentLibrary.content.Where(elem => elem.itemData==Content[ItemIndex]).First();
                equipmentLibraryItem.itemPrefab.SetActive(false);
            }
            if (ItemIndex != 0) ItemIndex--;
            else ItemIndex = InventorySize - 1;
            if (!IsTheCurrSlotFree())
            {
                equipmentLibraryItem=equipmentLibrary.content.Where(elem => elem.itemData==Content[ItemIndex]).First();
                equipmentLibraryItem.itemPrefab.SetActive(true);
            }
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
        if (IsTheCurrSlotFree())
        {
            Content[ItemIndex] = item;
            equipmentLibraryItem=equipmentLibrary.content.Where(elem => elem.itemData==item).First();
            equipmentLibraryItem.itemPrefab.SetActive(true);
        }
        else
        {
            // Instantiate(Content.GetValue(CurrSelectedItem));
            // Content.SetValue(item, CurrSelectedItem);
        }
    }
    
    public void RemoveItem()
    {
        equipmentLibraryItem=equipmentLibrary.content.Where(elem => elem.itemData==Content[ItemIndex]).First();
        equipmentLibraryItem.itemPrefab.SetActive(false);
        Content[ItemIndex] = null;
    }
    
    
}