using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    [SerializeField] private float sprintSpeed = 10f; // Sprinting speed
    [SerializeField] private float jumpForce = 5f; // Jump force
    [SerializeField] private float mouseSensitivity = 2f; // Mouse sensitivity
    [SerializeField] private float zoomSensitivity = 15f; // Zoom sensitivity
    [SerializeField] private float minZoom = 20f; // Minimum camera zoom (FOV)
    [SerializeField] private float maxZoom = 60f; // Maximum camera zoom (FOV)
    [SerializeField] private int maxJumps = 2; // Maximum number of jumps (e.g., 2 for double jump)

    private Rigidbody rb;
    private Camera playerCamera;
    private Vector3 movement;
    private float mouseX, mouseY;
    private bool isSprinting = false;
    private int jumpCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>(); // Assuming the camera is a child of the player
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of screen
    }

    void Update()
    {
        // Player movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Check if sprint key is pressed
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Check if there's any movement input
        if (horizontalInput != 0 || verticalInput != 0)
        {
            float currentSpeed = isSprinting ? sprintSpeed : speed;
            movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * currentSpeed;
        }
        else
        {
            movement = Vector3.zero; // No movement input, reset movement vector
        }

        // Mouse look input
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        mouseY = Mathf.Clamp(mouseY, -90f, 90f); // Limit vertical rotation

        // Rotate the player horizontally, and the camera vertically
        transform.localRotation = Quaternion.Euler(0f, mouseX, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(mouseY, 0f, 0f);

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Mouse zoom input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            ZoomCamera(scrollInput);
        }
    }

    void FixedUpdate()
    {
        // Move the player
        PerformMovement(movement);
    }

    void PerformMovement(Vector3 move)
    {
        rb.MovePosition(rb.position + transform.TransformDirection(move) * Time.fixedDeltaTime);
    }

    void Jump()
    {
        if (IsGrounded() || jumpCount < maxJumps)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }
    }

    bool IsGrounded()
    {
        // Raycast to check if the player is grounded
        RaycastHit hit;
        float distanceToGround = 0.1f;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, distanceToGround))
        {
            jumpCount = 0; // Reset jump count when grounded
            return true;
        }
        return false;
    }

    void ZoomCamera(float scrollInput)
    {
        float newFOV = playerCamera.fieldOfView - scrollInput * zoomSensitivity;
        playerCamera.fieldOfView = Mathf.Clamp(newFOV, minZoom, maxZoom);
    }
}



