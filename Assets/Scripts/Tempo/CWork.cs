using UnityEngine;
using Photon.Pun;

public class CWork : MonoBehaviourPunCallbacks
{
    public float sensitivity = 1f; // Sensibilité de la souris
    public float distance = 7f; // Distance fixe de la caméra par rapport au personnage

    private float _currentX = 0f; // Rotation en X (gauche/droite)
    private float _currentY = 0f; // Rotation en Y (haut/bas)
    private Transform _cameraTransform;
    private float _dist;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            // Désactiver ce script si ce n'est pas le joueur local
            enabled = false;
            return;
        }

        _cameraTransform = Camera.main!.transform;
        _dist = distance;
    }

    private void Update()
    {
        if (!photonView.IsMine || GetComponent<Player>().IsInAction)
            return;

        _currentX += Input.GetAxis("Mouse X") * sensitivity; // Mise à jour de la rotation en X
        _currentY -= Input.GetAxis("Mouse Y") * sensitivity; // Mise à jour de la rotation en Y
        
        if (!Debug.isDebugBuild)
        {
            _currentY = Mathf.Clamp(_currentY, -10,80); // Limitation de l'angle de rotation en Y
        }
    }

    private void LateUpdate()
    {
        if (!photonView.IsMine)
            return;

        Vector3 direction = new Vector3(0, 0, -distance); // Direction de la caméra
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0); // Calcul de la rotation de la caméra
        Vector3 desiredPosition = transform.position + Vector3.up * 2 + rotation * direction; // Position désirée de la caméra
        
        Debug.DrawLine(transform.position + Vector3.up * 2, desiredPosition - Vector3.up * 0.7f);
        Debug.DrawLine(transform.position + Vector3.up * 2, transform.position + Vector3.up * 2 + rotation * Vector3.forward * (-distance -0.5f) - Vector3.up * 0.7f, Color.red);
        Debug.DrawLine(transform.position + Vector3.up * 2, transform.position + Vector3.up * 2 + rotation * Vector3.forward * (-distance +0.1f) - Vector3.up * 0.7f, Color.green);

        // Assurez-vous que la caméra ne rentre pas dans le terrain ou d'autres objets
        if (Physics.Linecast(transform.position + Vector3.up * 2, desiredPosition - Vector3.up * 0.7f) && distance > 2)
        {
            if (Physics.Linecast(transform.position + Vector3.up * 2, transform.position + Vector3.up * 2 + rotation * Vector3.forward * (-distance +0.1f) - Vector3.up * 0.7f))
            {
                distance -= 0.3f;
            }
            else
            {
                distance -= 0.05f;
            }
        }
        else
        {
            if (!Physics.Linecast(transform.position + Vector3.up * 2, transform.position + Vector3.up * 2 + rotation * Vector3.forward * (-distance -0.5f) - Vector3.up * 0.7f))
            {
                if (distance < _dist)
                {
                    distance += 0.05f;
                }
            }
        }
        
        _cameraTransform.position = transform.position + Vector3.up * 2 + rotation * direction;
        
        _cameraTransform.LookAt(transform.position + Vector3.up * 2); // Faire en sorte que la caméra regarde le personnage
    }
}
