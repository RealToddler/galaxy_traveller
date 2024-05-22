using UnityEngine;
using Photon.Pun;

public class CWork : MonoBehaviourPunCallbacks
{
    public float sensitivity = 2f; // Sensibilité de la souris
    public float distance = 5f; // Distance fixe de la caméra par rapport au personnage

    private float currentX = 0f; // Rotation en X (gauche/droite)
    private float currentY = 0f; // Rotation en Y (haut/bas)

    private Transform cameraTransform;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            // Désactiver ce script si ce n'est pas le joueur local
            enabled = false;
            return;
        }

        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        currentX += Input.GetAxis("Mouse X") * sensitivity; // Mise à jour de la rotation en X
        currentY -= Input.GetAxis("Mouse Y") * sensitivity; // Mise à jour de la rotation en Y
        // currentY = Mathf.Clamp(currentY, -180, 180); // Limitation de l'angle de rotation en Y
    }

    private void LateUpdate()
    {
        if (!photonView.IsMine)
            return;

        Vector3 direction = new Vector3(0, 0, -distance); // Direction de la caméra
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0); // Calcul de la rotation de la caméra
        Vector3 desiredPosition = transform.position + rotation * direction; // Position désirée de la caméra

        // Assurez-vous que la caméra ne rentre pas dans le terrain ou d'autres objets
        RaycastHit hit;
        if (Physics.Linecast(transform.position , desiredPosition, out hit))
        {
            cameraTransform.position = hit.point;
        }
        else
        {
            cameraTransform.position = desiredPosition;
        }

        cameraTransform.LookAt(transform.position + Vector3.up * 2); // Faire en sorte que la caméra regarde le personnage
    }
}
