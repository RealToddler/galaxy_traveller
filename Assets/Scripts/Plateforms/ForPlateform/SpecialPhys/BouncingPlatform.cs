using UnityEngine;

public class BouncingPlatform : MonoBehaviour
{
    private float bounceForce = 15f; // Adjust the force value as needed
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<MoveBehaviour>().Bounce(bounceForce);
        }
    }
}