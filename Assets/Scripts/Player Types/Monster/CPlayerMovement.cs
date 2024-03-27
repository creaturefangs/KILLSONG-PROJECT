using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float jumpForce = 5f; // Jump force
    public float mouseSensitivity = 2f; // Mouse sensitivity

    private Rigidbody rb;
    private Camera playerCamera;
    private Vector3 movement;
    private float mouseX, mouseY;

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

        // Check if there's any movement input
        if (horizontalInput != 0 || verticalInput != 0)
        {
            movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * speed;
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
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        // Raycast to check if the player is grounded
        RaycastHit hit;
        float distanceToGround = 0.1f;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, distanceToGround))
        {
            return true;
        }
        return false;
    }
}
