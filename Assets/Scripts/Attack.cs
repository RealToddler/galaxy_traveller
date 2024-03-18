using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private Player player;
    [SerializeField] private Animation _animation;
    [SerializeField] private float damage;
    
    public void LaunchAttack()
    {
        Debug.Log("attack launched");
        player.GetDamage(damage);
    }
}
