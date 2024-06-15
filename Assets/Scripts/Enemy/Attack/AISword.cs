using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AISword : MonoBehaviour
{
    [SerializeField] private Enemy _launcher;
    private void OnTriggerEnter(Collider obj)
    {
        print("e");
        if (obj.gameObject.CompareTag("Player") && _launcher.CanAttack )
        {
            Player player= obj.gameObject.GetComponent<Player>();
            if (!player.IsHit) 
            {
                player.TakeDamage(_launcher.damage);
                player.PlayerAnimator.SetTrigger("Knockback");
            }
        }
    }
}
