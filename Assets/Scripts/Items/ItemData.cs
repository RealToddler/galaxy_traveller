using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/New item")]
public class ItemData : ScriptableObject
{
    public new string name;
    public Sprite visual;
    public GameObject prefab;
}
