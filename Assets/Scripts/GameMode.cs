using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public bool IsMultiPlayer { get; set; }

    private void Start()
    {
        DontDestroyOnLoad(this);
        //ok
    }
}
