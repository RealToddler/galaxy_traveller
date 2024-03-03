using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemData[] Content { get; private set; }

    public bool IsFree()
    {
        return Content.GetValue(CurrSelectedItem).IsUnityNull();
    }

    public static int InventorySize => 4;
    public int CurrSelectedItem { get; private set; }

    private void Start()
    {
        Content = new ItemData[4];
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            if (CurrSelectedItem != InventorySize-1) CurrSelectedItem++;
            else CurrSelectedItem = 0;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            if (CurrSelectedItem != 0) CurrSelectedItem--;
            else CurrSelectedItem = InventorySize - 1;
        }
    }

    public void AddItem(ItemData item)
    {
        if (IsFree())
        {
            Content.SetValue(item, CurrSelectedItem);
        }
        else
        {
            // Instantiate(Content.GetValue(CurrSelectedItem));
            // Content.SetValue(item, CurrSelectedItem);
        }
    }
    
    
}