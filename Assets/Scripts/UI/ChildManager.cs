using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class ChildManager : MonoBehaviour
{
    [SerializeField] private Object child;
    [SerializeField] private Object linkedWith;

    private void Update()
    {
        child.GameObject().SetActive(linkedWith.GameObject().activeSelf);
    }
}
