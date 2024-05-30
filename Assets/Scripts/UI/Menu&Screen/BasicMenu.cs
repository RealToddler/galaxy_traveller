using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicMenu : MonoBehaviour
{
    public void SwitchTo(BasicMenu targetMenu)
    {
        this.GameObject().SetActive(false);
        targetMenu.GameObject().SetActive(true);
    }
    
    public void SwitchToMenus()
    {
        SceneManager.LoadScene("Menus");
    }
}
