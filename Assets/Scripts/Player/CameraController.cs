using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float cameraDistance = 7.0f;
    public float cameraHeight = 3.0f;
    public Vector3 cameraOffset = Vector3.zero;
    
    private Transform cameraTransform;

    void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 desiredPosition = transform.position - transform.forward * cameraDistance + Vector3.up * cameraHeight;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, Time.deltaTime * 5.0f);
        cameraTransform.LookAt(transform.position + cameraOffset);
    }
}