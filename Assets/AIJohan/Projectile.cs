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
                player.KnockBack(20);
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
        else if (obj.gameObject.CompareTag("Cible"))
        {
            obj.gameObject.GetComponent<Cible>().ChangeColor();
        }
    }
    void Start()
    {
        Destroy(gameObject,1f);
        AudioManager.Instance.Play("Gun");
    }
}
