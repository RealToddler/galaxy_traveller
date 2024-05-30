using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : Attack
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _gun;
    [SerializeField] private Transform _laserOrigin;
    private float gunRange = 50f;
    private float laserDuration = 0.05f;
    LineRenderer laserLine;
    void Start()
    {
        _name = "Distance";
        _damage = _launcher.distanceDamage;
        laserLine = _gun.GetComponent<LineRenderer>();
    }

    public override void LaunchAttack()
    {
        
        
        if (_launcher.nbshots>0 && _launcher.platform.players[_launcher.IndexNearestPlayer()].GetComponent<Player>().Health>0)
        {
            print("distance");
            Vector3 rayOrigin = _laserOrigin.position;
            RaycastHit hit;
            if(Physics.Raycast(_launcher.transform.position + new Vector3(0,1,0), transform.forward, out hit, 8f))
            {
                if (_launcher.nbshots>0)
                {
                    laserLine.SetPosition(0, _laserOrigin.position);
                    laserLine.SetPosition(1, hit.point);
                    if (hit.transform.CompareTag("Player"))
                    {
                        hit.collider.GetComponent<Player>().TakeDamage(_damage);
                    }
                }
            }
            else
            {
                laserLine.SetPosition(0, _laserOrigin.position);
                laserLine.SetPosition(1, rayOrigin + (_laserOrigin.up*-1 * gunRange));
            }
            StartCoroutine(ShootLaser());
        }        
    }
    IEnumerator ShootLaser()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }
}
