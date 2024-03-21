using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Attack/New attack")]
public class Attack : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Animation _animation;
    [SerializeField] private float damage;
    
    // public void LaunchAttack()
    // {
    //     Debug.Log("attack launched");
    //     player.GetDamage(damage);
    // }
}
