using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    protected string _name;
    protected float _damage;
    [SerializeField]protected  Enemy _launcher;
    public string Name ()
    {
        return _name;
    }
    public virtual void LaunchAttack(){}
}
