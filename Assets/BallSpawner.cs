using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private float startAfter;
    [SerializeField] private float spawnEvery = 4;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnBall), startAfter,spawnEvery);
    }

    private void SpawnBall()
    {
        Instantiate(ball, gameObject.transform);
    }
}
