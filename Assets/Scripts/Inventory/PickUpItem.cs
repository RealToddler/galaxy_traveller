using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] 
    private float pickUpRange = 3.5f;
    
    public PickupBehaviour playerPickupBehaviour;

    void Update()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange))
        {
            if (hit.transform.CompareTag("Item"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerPickupBehaviour.DoPickup(hit.transform.gameObject.GetComponent<Item>());
                }
            }
        }
    }
}
