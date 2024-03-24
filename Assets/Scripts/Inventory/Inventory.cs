using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemData[] Content { get; private set; }

    public static int InventorySize => 4;
    public int ItemIndex { get; private set; }

    private void Start()
    {
        Content = new ItemData[4];
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            if (ItemIndex != InventorySize-1) ItemIndex++;
            else ItemIndex = 0;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            if (ItemIndex != 0) ItemIndex--;
            else ItemIndex = InventorySize - 1;
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
        }
        else
        {
            // Instantiate(Content.GetValue(CurrSelectedItem));
            // Content.SetValue(item, CurrSelectedItem);
        }
    }
    
    public void RemoveItem()
    {
        Content[ItemIndex] = null;
    }
    
    
}