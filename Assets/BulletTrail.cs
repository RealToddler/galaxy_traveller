using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3[] positions = new Vector3[2];
    private float timeOfInstantiation;
    private Rigidbody rigidbody;
    private Vector3 _spawnpoint;

    void Start()
    {
        _spawnpoint=transform.position; 
        timeOfInstantiation=Time.time;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.yellow;
        rigidbody=GetComponent<Rigidbody>();
    }

    void Update()
    {
        float delay=Time.time - timeOfInstantiation;
        if (delay>0.02)
        {
            positions[0] = transform.position;
            positions[1] = _spawnpoint;
            lineRenderer.SetPositions(positions);
        }
        
    }
}
