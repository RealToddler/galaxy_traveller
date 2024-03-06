using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    public void SwitchTo(Menu targetMenu)
    {
        this.GameObject().SetActive(false);
        targetMenu.GameObject().SetActive(true);
    }
}
