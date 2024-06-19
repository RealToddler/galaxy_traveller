using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3[] positions = new Vector3[2];
    private float timeOfInstantiation;
    private Vector3 _spawnpoint;

    void Start()
    {
        Destroy(gameObject,1f);
        AudioManager.Instance.Play("Gun");
        
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
        if (Physics.Linecast(_spawnpoint, transform.position, out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Player player= hit.collider.gameObject.GetComponent<Player>();
                if (!player.IsHit) 
                {
                    player.KnockBack(20);
                }
            }
            else if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.gameObject.GetComponentInParent<Enemy>();
                if (enemy != null)
                {
                    if (!enemy.IsHit) 
                    {
                        enemy.LooseHealth(60);
                        enemy.KnockBack();
                    }
                }
            }
            
            Destroy(gameObject);
        }
        
        // Train
        float delay = Time.time - timeOfInstantiation;
        if (delay>0.02)
        {
            positions[0] = transform.position;
            lineRenderer.SetPositions(positions);
        }
    }
}
