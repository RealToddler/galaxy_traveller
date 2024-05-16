using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6.0f;
    public float sprintMultiplier = 1.5f;
    public float rotationSpeed = 2.0f;

    [Header("Camera Settings")]
    public float cameraDistance = 7.0f;
    public float cameraHeight = 3.0f;
    public Vector3 cameraOffset = Vector3.zero;
    
    private Transform cameraTransform;
    private Rigidbody playerRigidbody;
    private Animator animator;
    private bool isGrounded;
    private bool isSprinting;

    // Cached animator hash IDs
    private int hFloat;
    private int vFloat;
    private int groundedBool;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;

        hFloat = Animator.StringToHash("H");
        vFloat = Animator.StringToHash("V");
        groundedBool = Animator.StringToHash("Grounded");
    }

    void Update()
    {
        HandleInput();
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
        CheckGroundStatus();
    }

    void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void HandleInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        isSprinting = Input.GetButton("Sprint");

        animator.SetFloat(hFloat, h);
        animator.SetFloat(vFloat, v);
        animator.SetBool(groundedBool, isGrounded);

        float curSpeed = Vector2.ClampMagnitude(new Vector2(h, v), 1f).magnitude;
        animator.SetFloat("Speed", curSpeed);
    }

    private void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (movement.magnitude > 1) movement.Normalize();
        movement = transform.TransformDirection(movement);
        movement *= speed * (isSprinting ? sprintMultiplier : 1);
        
        playerRigidbody.MovePosition(playerRigidbody.position + movement * Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.Rotate(0, horizontalRotation, 0);
    }

    private void UpdateAnimator()
    {
        animator.SetFloat(hFloat, Input.GetAxis("Horizontal"), 0.1f, Time.deltaTime);
        animator.SetFloat(vFloat, Input.GetAxis("Vertical"), 0.1f, Time.deltaTime);
    }

    private void UpdateCameraPosition()
    {
        Vector3 desiredPosition = transform.position - transform.forward * cameraDistance + Vector3.up * cameraHeight;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, Time.deltaTime * 5.0f);
        cameraTransform.LookAt(transform.position + cameraOffset);
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, 0.1f);
        animator.SetBool(groundedBool, isGrounded);
    }
}