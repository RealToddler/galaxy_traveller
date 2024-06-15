using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public string Name { get; set; } = "";
    protected float Damage;
    
    [SerializeField] protected  Enemy launcher;

    public virtual void LaunchAttack(){}
}
