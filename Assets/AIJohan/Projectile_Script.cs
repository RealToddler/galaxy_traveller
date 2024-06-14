using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile_Script : MonoBehaviour
{
    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            Player player= obj.collider.gameObject.GetComponent<Player>();
            if (!player.IsHit) 
            {
                player.TakeDamage(20);
                player.PlayerAnimator.SetTrigger("Knockback");
            }
        }
        else if (obj.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy=obj.collider.gameObject.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                if (!enemy.isHit) 
                {
                    enemy.LooseHealth(60);
                    enemy.IAAnimator.SetTrigger("Knockback");
                }
            }
            Destroy(this.gameObject);
        }
    }
}
