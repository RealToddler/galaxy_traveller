using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    
    //[SerializeField] private Animation _animation;
    [SerializeField] protected EnemyDistance _launcher;
    protected string _name;
    protected float _damage;
    public string Name ()
    {
        return _name;
    }
    public abstract void LaunchAttack();
}
