using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenu : BasicMenu
{
    private void Update()
    {
        if (!Cursor.visible)
        {
            Cursor.visible = true;
        }
    }
}
