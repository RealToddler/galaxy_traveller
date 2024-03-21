using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float CooldownAppear;
    [SerializeField] float CooldownDisapear;

    private void OnCollisionEnter(Collision collision)
    {
        Invoke(nameof(Disapear), CooldownDisapear);
    }

    private void OnCollisionExit(Collision collision)
    {
        Invoke(nameof(Appear), CooldownAppear);
    }

    private void Disapear() 
    {
        gameObject.SetActive(false);
        Invoke(nameof(Appear), CooldownAppear);
    }

    private void Appear()
    {
        gameObject.SetActive(true);
    }
}