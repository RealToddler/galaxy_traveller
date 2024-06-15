using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private Player _launcher;
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Enemy") && _launcher.CanAttack && _launcher.HasHit)
        {
            
            Enemy enemy= obj.GetComponentInParent<Enemy>();
            if (!enemy.IsHit)
            {
                enemy.LooseHealth(25);
                enemy.KnockBack();
            }
        }
    }
}
