using UnityEngine;

public class BouncingPlatform : MonoBehaviour
{
    [SerializeField] private float bounceForce = 15f;
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<MoveBehaviour>().Bounce(bounceForce);
        }
    }
}