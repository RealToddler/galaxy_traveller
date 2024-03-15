using System.Collections;
using UnityEngine;

public class StickyUp : MonoBehaviour
{

    private float waitingtime = 0.5f;
    
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.SetParent(transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.transform.parent != null)
        {
            StartCoroutine(ExitCollision(collision));
        }
    }
    
    IEnumerator ExitCollision(Collision collision)
    {
        yield return new WaitForSecondsRealtime(waitingtime);
        collision.gameObject.transform.SetParent(null);
    }
}
