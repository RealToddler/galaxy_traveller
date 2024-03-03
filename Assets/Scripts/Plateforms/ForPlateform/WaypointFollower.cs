using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] GameObject[] waypoints;
    int currentWaypointIndex = 0;

    [SerializeField] float speed = 1f;

    bool isWaiting = false;
    void Update()
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
        if (!isWaiting) transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
    }

    IEnumerator WaitAndChangePoint()
    {
        isWaiting = true;

        yield return new WaitForSecondsRealtime(waypoints[currentWaypointIndex].GetComponent<WayPoint>().waitTime);

        isWaiting = false;
    }
}
