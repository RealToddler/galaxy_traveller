using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Attack
{
    // Start is called before the first frame update
    void Start()
    {
        Name = "Melee";
        Damage = launcher.damage;
    }
    
}
