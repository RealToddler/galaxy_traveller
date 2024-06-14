using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private Player _launcher;
    private void OnTriggerEnter(Collider obj)
    {
        print(_launcher.HasHit);
        if (obj.gameObject.CompareTag("Enemy") && _launcher.IsInAction && _launcher.HasHit)
        {
            
            Enemy enemy= obj.GetComponentInParent<Enemy>();
            if (!enemy.isHit)
            {
                enemy.LooseHealth(25);
                enemy.IAAnimator.SetTrigger("Knockback");
            }
        }
    }
}
