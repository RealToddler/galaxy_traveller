using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
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
                if (!enemy.IsHit) 
                {
                    enemy.LooseHealth(60);
                    enemy.KnockBack();
                }
            }
            
            Destroy(gameObject);
        }
    }
}
