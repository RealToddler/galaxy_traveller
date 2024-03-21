using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool activeOnContact;

    private bool active;
    private int currentWaypointIndex;
    private bool isWaiting;

    void Update()
    {
        if (!activeOnContact || active)
        {
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].transform.position) < .1f)
            {
                StartCoroutine(WaitAndChangePoint());
                
                currentWaypointIndex++;

                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;
                }
            }

            if (!isWaiting)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
            }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        active = true;
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (activeOnContact)
        {
            active = false;
        }
    }

    IEnumerator WaitAndChangePoint()
    {
        isWaiting = true;

        yield return new WaitForSecondsRealtime(waypoints[currentWaypointIndex].GetComponent<WayPoint>().waitTime);

        isWaiting = false;
    }
}
