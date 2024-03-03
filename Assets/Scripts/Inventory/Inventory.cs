using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<ItemData> Content { get; private set; }
    public bool IsFull() => InventorySize == Content.Count;

    private const int InventorySize = 4;
    private ItemData _currSelectedItem;

    private void Start()
    {
        Content = new List<ItemData>();
    }
    
    public void AddItem(ItemData item)
    {
        Content.Add(item);
    }
    
    
}