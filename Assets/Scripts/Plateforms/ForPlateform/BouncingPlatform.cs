using UnityEngine;

public class BouncingPlatform : MonoBehaviour
{
    [SerializeField] private float height = 15f;
    [SerializeField] private Rigidbody playerRigidBody;
    [SerializeField] private GameObject player;
    void OnCollisionEnter()
    {
        player.transform.up = Vector3.up * height;
    }
}
