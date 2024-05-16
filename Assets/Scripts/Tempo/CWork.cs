using UnityEngine;

public class CWork : MonoBehaviour
{
    [SerializeField] private float distance = 7.0f;
    [SerializeField] private float height = 3.0f;
    [SerializeField] private Vector3 centerOffset = Vector3.zero;
    [SerializeField] private float rotationSpeed = 2.0f;
    
    private Transform cameraTransform;
    private Transform playerTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        playerTransform = transform; // Assuming the script is attached to the player GameObject
    }

    void LateUpdate()
    {
        if (GetComponent<MoveBehaviour>()._speed > 0.1f)
        {
            // Rotate the player based on mouse movement
            float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            playerTransform.Rotate(0, horizontalRotation, 0);
            
            // Calculate camera position based on player position and distance
            Vector3 desiredPosition = playerTransform.position - playerTransform.forward * distance;
            desiredPosition.y = playerTransform.position.y + height;
            
            // Smoothly move the camera to the desired position
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, Time.deltaTime * 5.0f);
        }
        else
        {
            // Calculate camera position based on player position and distance
            Vector3 desiredPosition = playerTransform.position;
            desiredPosition.y = playerTransform.position.y + height;
            
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, Time.deltaTime * 0.5f);
        }
        
        // Make the camera look at the player
        cameraTransform.LookAt(playerTransform.position + centerOffset);
    }
}