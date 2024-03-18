using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAfterEnnemyDeath : MonoBehaviour
{
    [SerializeField] public Ennemy ennemy;
    [SerializeField] private GameObject platForm;

    private void Update()
    {
        if (ennemy.Health <= 0)
        {
            platForm.gameObject.SetActive(true);
        }
    }
}
