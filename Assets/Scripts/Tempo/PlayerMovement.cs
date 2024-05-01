using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f; // Vitesse de déplacement du joueur

    private Rigidbody rb; // Référence au composant Rigidbody

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Récupération du composant Rigidbody
    }

    void Update()
    {
        // Récupération des inputs du clavier
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Création du vecteur de déplacement
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Application du mouvement au Rigidbody
        rb.MovePosition(transform.position + movement * speed * Time.deltaTime);
    }
}
