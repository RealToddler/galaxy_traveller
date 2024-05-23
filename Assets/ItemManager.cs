using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] items;
    
    void Start()
    {
        items = GameObject.FindGameObjectsWithTag("Item");
    }

    void Update()
    {
        
    }
}
