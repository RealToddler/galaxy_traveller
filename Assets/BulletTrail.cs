using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3[] positions = new Vector3[2];
    private float timeOfInstantiation;
    private Vector3 _spawnpoint;

    void Start()
    {
        _spawnpoint = transform.position; 
        timeOfInstantiation = Time.time;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0;
        lineRenderer.positionCount = 2;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = new Color(255, 255, 0, 0);
        positions[1] = _spawnpoint;
    }

    void Update()
    {
        float delay = Time.time - timeOfInstantiation;
        if (delay>0.02)
        {
            positions[0] = transform.position;
            lineRenderer.SetPositions(positions);
        }
    }
}
