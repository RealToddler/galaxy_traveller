using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cible : MonoBehaviour
{
   public void ChangeColor()
   {
      Material mat = GetComponent<Material>();
      
      if (mat.color == Color.red)
      {
         mat.SetColor(Color.green);
      }
      else
      {
         mat.color = Color.red;
      }
   }
}
