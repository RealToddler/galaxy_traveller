using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAfterAIDeath : MonoBehaviour
{
    [SerializeField] private Ennemy ai;
    [SerializeField] private GameObject platForm;

    private void Update()
    {
        if (ai.Health <= 0)
        {
            platForm.gameObject.SetActive(true);
        }
    }
}
