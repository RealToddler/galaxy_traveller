using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private string _name;
    private Animation _animation;

    public Attack(string name, Animation animation)
    {
        _name = name;
        _animation = animation;
    }
    
    public void LaunchAttack()
    {
        // a faire
    }
}
