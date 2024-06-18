using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;


public class despawn : MonoBehaviour
{

    [SerializeField] private List<GameObject> list = new List<GameObject>();

    
    async void Start()
    {
        
        begin(0,2,1);
        await Task.Delay(3000);
        begin(3,4,5);
        await Task.Delay(2000);
        begin(8,7,6);
        await Task.Delay(300);
        begin(9,10,11);
    }

    async void begin(int a, int b, int c)
    {
        Despawn(a);
        await Task.Delay(1000);
        Despawn(c);
        await Task.Delay(300);
        Despawn(b);
    }
    
    private async void Despawn(int c)
    {
        if (list[c].activeSelf)
        {
            list[c].SetActive(false);
            await Task.Delay(5000);
            list[c].SetActive(true);
            await Task.Delay(4000);
            Despawn(c);
        }
    }
}
