using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicMenu : MonoBehaviour
{
    public void SwitchTo(BasicMenu targetMenu)
    {
        this.GameObject().SetActive(false);
        targetMenu.GameObject().SetActive(true);
    }
}
